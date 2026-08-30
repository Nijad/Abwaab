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

namespace Abwaab.Application.Features.Appointments.Commands.Complete;

public class CompleteAppointmentCommandHandler : IRequestHandler<CompleteAppointmentCommand, CompleteAppointmentResponse>
{
    private readonly IUserService _userService;
    private readonly IAppointmentService _appointmentService;
    private readonly INotificationService _notificationService;
    private readonly INotifyHandler _notifyHandler;

    private readonly string errorTitle = ErrorTitle.ConfirmAppointment;

    public CompleteAppointmentCommandHandler(
        IUserService userService,
        IAppointmentService appointmentService,
        INotificationService notificationService,
        INotifyHandler notifyHandler)
    {
        _userService = userService;
        _appointmentService = appointmentService;
        _notificationService = notificationService;
        _notifyHandler = notifyHandler;
    }

    public async Task<CompleteAppointmentResponse> Handle(CompleteAppointmentCommand request, CancellationToken cancellationToken)
    {
        //get current user from context
        string username = _userService.FindUserNameByContext(errorTitle);
        ApplicationUser? user = await _userService.FindUserByNameAsync(username);
        if (user == null)
            throw new UserNotFoundException(username, errorTitle);

        //get property by Id
        Appointment appointment = await _appointmentService.FindAppointmentByIdAsync(request.AppointmentId, errorTitle, cancellationToken);

        ApplicationUser secondParty;
        //check if the user is the same as the owner or the visitor
        if (user == appointment.User)
            secondParty = appointment.Property.UserPlan.User;
        else if(user == appointment.Property.UserPlan.User)
            secondParty = appointment.User;
        else
            throw new ObjectNotBelongToUserException("الموعد" ,errorTitle);

        //check whether the appointment status is confirmed
        AppointmentState confirmedAppointmentState = await _appointmentService.GetConfirmedAppointmentStateAsync(errorTitle);

        if(appointment.AppointmentState !=  confirmedAppointmentState)
            throw new ChanginAppointmentStateNotAllowedException($"لا يمكنك إتمام موعد وهو في حالة '{AppointmentStatesMapping.Map(appointment.AppointmentState.StateName)}'", errorTitle);

        //check that there are more than 6 hours remaining until the appointment.
        if (appointment.Date < DateTime.Now)
            throw new ChanginAppointmentStateNotAllowedException($"لا يمكنك إتمام موعد قبل موعده", errorTitle);

        //get cancel state
        AppointmentState canceledAppointmentState = await _appointmentService.GetCanceledAppointmentStateAsync(errorTitle);

        //update the appointment
        appointment.AppointmentState = canceledAppointmentState;
        appointment.UserComments = request.Comment;
        await _appointmentService.UpdateAppointmentAsync(appointment, cancellationToken);

        //notify the second party
        List<ApplicationUser> users = new() { secondParty };
        List<Notification> notifications = await _notificationService.InitiateNotifications("لقد تم تسجيل إتمام الموعد، يمكنكم الاطلاع على مزيد من التفاصيل على الموقع الالكتروني", users, errorTitle);

        await _notifyHandler.NotifyAsync(errorTitle);

        //return response
        return new CompleteAppointmentResponse() { Success = true, Message = "تم إتمام الموعد بنجاح." };
    }
}