using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Appointments;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Contracts;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.AppointmentEntities;
using Abwaab.Domain.Entities.NotificationEntities;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;

namespace Abwaab.Application.Features.Appointments.Confirm;

public class ConfirmAppointmentCommandHandler : IRequestHandler<ConfirmAppointmentCommand, ConfirmAppointmentResponse>
{
    private readonly IAppointmentService _appointmentService;
    private readonly IUserService _userService;
    private readonly INotificationService _notificationService;
    private readonly INotifyHandler _notifyHandler;

    private readonly string errorTitle = ErrorTitle.ConfirmAppointment;

    public ConfirmAppointmentCommandHandler(
        IAppointmentService appointmentService,
        IUserService userService,
        INotificationService notificationService,
        INotifyHandler notifyHandler)
    {
        _appointmentService = appointmentService;
        _userService = userService;
        _notificationService = notificationService;
        _notifyHandler = notifyHandler;
    }

    public async Task<ConfirmAppointmentResponse> Handle(ConfirmAppointmentCommand request, CancellationToken cancellationToken)
    {
        //get appointment
        Appointment appointment = await _appointmentService.FindAppointmentByIdAsync(request.AppointmentId ,errorTitle, cancellationToken);

        //check user(owner)
        string username = _userService.FindUserNameByContext(errorTitle);
        ApplicationUser? user = await _userService.FindUserByNameAsync(username);
        if (user == null)
            throw new UserNotFoundException(username, errorTitle);

        //check if appointment belong to the user
        if (user.Id != appointment.Property.UserPlan.UserId)
            throw new ObjectNotBelongToUserException("الموعد", errorTitle);

        //check appointment state if pending
        AppointmentState pendingAppointmentState = await _appointmentService.GetPendingAppointmentStateAsync(errorTitle);

        if(appointment.AppointmentStateId != pendingAppointmentState.Id)
            throw new ConfirmationAppointmentNotAllowedException(errorTitle);

        //check if appointment date in future
        if (appointment.Date < DateTime.Now)
            throw new ConfirmationAppointmentNotAllowedException(errorTitle);

        //get confirmed state
        AppointmentState ConfirmedAppointmentState = await _appointmentService.GetConfirmedAppointmentStateAsync(errorTitle);

        //update appointment
        appointment.AppointmentState = ConfirmedAppointmentState;
        await _appointmentService.UpdateAppointment(appointment, cancellationToken);

        //notify visitor and send owner contact to him
        List<ApplicationUser> users = new() { appointment.User };
        string msg = $"تم تأكيد الموعد، يمكنك التواصل مع صاحب العقار عن طريق ";
        if (!string.IsNullOrEmpty(user.Email))
            msg += $"البريد الالكتروني: {user.Email}";
        if (!string.IsNullOrEmpty(user.Email) && !string.IsNullOrEmpty(user.PhoneNumber))
            msg += " أو ";
        if (!string.IsNullOrEmpty(user.PhoneNumber))
            msg += $"رقم الهاتف: {user.PhoneNumber}";

        List<Notification> notifications = await _notificationService.InitiateNotifications(msg, users, errorTitle);

        await _notifyHandler.NotifyAsync(errorTitle);

        //return response
        return new ConfirmAppointmentResponse() { Success = true, Message = "تم تأكيد الموعد بنجاح." }; 
    }
}