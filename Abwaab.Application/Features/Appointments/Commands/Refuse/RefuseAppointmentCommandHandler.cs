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

namespace Abwaab.Application.Features.Appointments.Commands.Refuse;

public class RefuseAppointmentCommandHandler : IRequestHandler<RefuseAppointmentCommand, RefuseAppointmentResponse>
{
    private readonly IUserService _userService;
    private readonly IAppointmentService _appointmentService;
    private readonly INotificationService _notificationService;
    private readonly INotifyHandler _notifyHandler;

    private readonly string errorTitle = ErrorTitle.ConfirmAppointment;

    public RefuseAppointmentCommandHandler(
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

    public async Task<RefuseAppointmentResponse> Handle(RefuseAppointmentCommand request, CancellationToken cancellationToken)
    {
        //get current user from context
        string username = _userService.FindUserNameByContext(errorTitle);
        ApplicationUser? user = await _userService.FindUserByNameAsync(username);
        if (user == null)
            throw new UserNotFoundException(username, errorTitle);

        //get property by Id
        Appointment appointment = await _appointmentService.FindAppointmentByIdAsync(request.AppointmentId, errorTitle, cancellationToken);

        //check if the user is the same as the owner        
        if(user != appointment.Property.UserPlan.User)
            throw new ObjectNotBelongToUserException("الموعد" ,errorTitle);

        //check whether the appointment status is either pending
        AppointmentState pendingAppointmentState = await _appointmentService.GetPendingAppointmentStateAsync(errorTitle);

        if(appointment.AppointmentState != pendingAppointmentState)
            throw new ChanginAppointmentStateNotAllowedException($"لا يمكنك إلغاء رفض وهو في حالة '{AppointmentStatesMapping.Map(appointment.AppointmentState.StateName)}'", errorTitle);

        //get cancel state
        AppointmentState refusedAppointmentState = await _appointmentService.GetRefusedAppointmentStateAsync(errorTitle);

        //update the appointment
        appointment.AppointmentState = refusedAppointmentState;
        appointment.UserComments = request.Comment;
        await _appointmentService.UpdateAppointmentAsync(appointment, cancellationToken);

        //notify the second party
        List<ApplicationUser> users = new() { appointment.User };
        List<Notification> notifications = await _notificationService.InitiateNotifications("مالك العقار رفض طلبك بحجز موعد زيارة، يمكنكم الاطلاع على مزيد من التفاصيل على الموقع الالكتروني", users, errorTitle);

        await _notifyHandler.NotifyAsync(errorTitle);

        //return response
        return new RefuseAppointmentResponse() { Success = true, Message = "تم رفض الموعد بنجاح." };
    }
}