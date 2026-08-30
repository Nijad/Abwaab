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

namespace Abwaab.Application.Features.Appointments.Commands.Cancel;

public class CancelAppointmentCommandHandler : IRequestHandler<CancelAppointmentCommand, CancelAppointmentResponse>
{
    private readonly IUserService _userService;
    private readonly IAppointmentService _appointmentService;
    private readonly INotificationService _notificationService;
    private readonly INotifyHandler _notifyHandler;

    private readonly string errorTitle = ErrorTitle.ConfirmAppointment;

    public CancelAppointmentCommandHandler(
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

    public async Task<CancelAppointmentResponse> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
    {
        //get current user from context
        string username = _userService.FindUserNameByContext(errorTitle);
        ApplicationUser? user = await _userService.FindUserByNameAsync(username);
        if (user == null)
            throw new UserNotFoundException(username, errorTitle);

        //get property by Id
        Appointment appointment = await _appointmentService.FindAppointmentByIdAsync(request.AppointmentId, errorTitle, cancellationToken);

        ApplicationUser secondParty;
        bool isVisitor = false;
        //check if the user is the same as the owner or the visitor
        if (user == appointment.User)
        {
            secondParty = appointment.Property.UserPlan.User;
            isVisitor = true;
        }
        else if (user == appointment.Property.UserPlan.User)
            secondParty = appointment.User;
        else
            throw new ObjectNotBelongToUserException("الموعد", errorTitle);

        //check whether the appointment status is either pending or confirmed
        AppointmentState pendingAppointmentState = await _appointmentService.GetPendingAppointmentStateAsync(errorTitle);
        AppointmentState confirmedAppointmentState = await _appointmentService.GetConfirmedAppointmentStateAsync(errorTitle);

        // accept to cancel => confirmed or (visitor and pending)
        if (appointment.AppointmentState != confirmedAppointmentState)
            if (appointment.AppointmentState != pendingAppointmentState || !isVisitor)
                throw new ChanginAppointmentStateNotAllowedException($"لا يمكنك إلغاء موعد وهو في حالة '{AppointmentStatesMapping.Map(appointment.AppointmentState.StateName)}'", errorTitle);

        //check that there are more than 6 hours remaining until the appointment.
        if (appointment.Date < DateTime.Now.AddHours(GeneralConstants.HOURS_NOT_ALLOWED_CANCEL_APPOINTMENT))
            throw new ChanginAppointmentStateNotAllowedException($"لا يمكنك إلغاء الموعد قبل أقل من {GeneralConstants.HOURS_NOT_ALLOWED_CANCEL_APPOINTMENT} ساعات",errorTitle);

        //get cancel state
        AppointmentState canceledAppointmentState = await _appointmentService.GetCanceledAppointmentStateAsync(errorTitle);

        //update the appointment
        appointment.AppointmentState = canceledAppointmentState;
        appointment.UserComments = request.Comment;
        await _appointmentService.UpdateAppointmentAsync(appointment, cancellationToken);

        //notify the second party
        List<ApplicationUser> users = new() { secondParty };
        List<Notification> notifications = await _notificationService.InitiateNotifications("لقد تم إلغاء الموعد، يمكنكم الاطلاع على مزيد من التفاصيل على الموقع الالكتروني", users, errorTitle);

        await _notifyHandler.NotifyAsync(errorTitle);

        //return response
        return new CancelAppointmentResponse() { Success = true, Message = "تم إلغاء الموعد بنجاح." };
    }
}