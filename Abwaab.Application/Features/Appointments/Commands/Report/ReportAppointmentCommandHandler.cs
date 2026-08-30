using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Appointments;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Common.Mappings;
using Abwaab.Application.Contracts;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.AppointmentEntities;
using Abwaab.Domain.Entities.NotificationEntities;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Abwaab.Application.Features.Appointments.Commands.Report;

public class ReportAppointmentCommandHandler : IRequestHandler<ReportAppointmentCommand, ReportAppointmentResponse>
{
    private readonly IUserService _userService;
    private readonly IAppointmentService _appointmentService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITransactionManager _transactionManager;
    private readonly INotificationService _notificationService;
    private readonly INotifyHandler _notifyHandler;

    private readonly string errorTitle = ErrorTitle.ConfirmAppointment;

    public ReportAppointmentCommandHandler(
        IUserService userService,
        IAppointmentService appointmentService,
        UserManager<ApplicationUser> userManager,
        ITransactionManager transactionManager,
        INotificationService notificationService,
        INotifyHandler notifyHandler)
    {
        _userService = userService;
        _appointmentService = appointmentService;
        _userManager = userManager;
        _transactionManager = transactionManager;
        _notificationService = notificationService;
        _notifyHandler = notifyHandler;
    }

    public async Task<ReportAppointmentResponse> Handle(ReportAppointmentCommand request, CancellationToken cancellationToken)
    {
        //get current user from context
        string username = _userService.FindUserNameByContext(errorTitle);
        ApplicationUser? user = await _userService.FindUserByNameAsync(username);
        if (user == null)
            throw new UserNotFoundException(username, errorTitle);

        //get property by Id
        Appointment appointment = await _appointmentService.FindAppointmentByIdAsync(request.AppointmentId, errorTitle, cancellationToken);
                
        if(user != appointment.Property.UserPlan.User)
            throw new ObjectNotBelongToUserException("الموعد" ,errorTitle);

        //check whether the appointment status is either pending
        AppointmentState confirmedAppointmentState = await _appointmentService.GetConfirmedAppointmentStateAsync(errorTitle);

        if(appointment.AppointmentState != confirmedAppointmentState)
            throw new ChanginAppointmentStateNotAllowedException($"لا يمكنك الإبلاغ عن موعد وهو في حالة '{AppointmentStatesMapping.Map(appointment.AppointmentState.StateName)}'", errorTitle);

        //get cancel state
        AppointmentState unfinishedAppointmentState = await _appointmentService.GetUnfinishedAppointmentStateAsync(errorTitle);

        await _transactionManager.BeginTransactionAsync(cancellationToken);
        try
        {
            //update the appointment
            appointment.AppointmentState = unfinishedAppointmentState;
            appointment.UserComments = request.Comment;
            await _appointmentService.UpdateAppointmentAsync(appointment, cancellationToken);

            string msg = "لقد تم الإبلاغ عن عدم قدومك للموعد وعدم اعتذارك قبل 6 ساعات من وقت الموعد. إذا لم يكن ذلك صحيحاً يرجى التواصل مع إدارة الموقع عبر وسائل التواصل الخاصة.";

            appointment.User.ReportCount++;
            if (GeneralConstants.REPORTS_COUNT_TO_BLOCK_USER == appointment.User.ReportCount)
            {
                appointment.User.IsBlocked = true;
                msg = $"لقد تم حظرك من الموقع بسبب عدم قدومك إلى {GeneralConstants.REPORTS_COUNT_TO_BLOCK_USER} مواعيد دون اعتذار. إذا لم يكن ذلك صحيحاً يرجى التواصل مع إدارة الموقع عبر وسائل التواصل الخاصة.";
            }

            await _userManager.UpdateAsync(appointment.User);

            await _transactionManager.CommitTransactionAsync(cancellationToken);

            List<ApplicationUser> users = new() { appointment.User };
            List<Notification> notifications = await _notificationService.InitiateNotifications(msg, users, errorTitle);

            await _notifyHandler.NotifyAsync(errorTitle);

            //return response
            return new ReportAppointmentResponse() { Success = true, Message = "تم الإبلاغ عن الموعد بنجاح." };
        }
        catch
        {
            await _transactionManager.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}