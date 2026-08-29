using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Appointments;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Contracts;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.AppointmentEntities;
using Abwaab.Domain.Entities.NotificationEntities;
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;

namespace Abwaab.Application.Features.Appointments.Book;

public class BookAppointmentCommandHandler : IRequestHandler<BookAppointmentCommand, BookAppointmentResponse>
{
    private readonly IUserService _userService;
    private readonly IPropertyService _propertyService;
    private readonly IPropertyStatesService _propertyStatesService;
    private readonly IAppointmentService _appointmentService;
    private readonly INotificationService _notificationService;
    private readonly INotifyHandler _notifyHandler;
    private readonly string errorTitle = ErrorTitle.BookAppointment;

    public BookAppointmentCommandHandler(IUserService userService, IPropertyService propertyService, IPropertyStatesService propertyStatesService, IAppointmentService appointmentService, INotificationService notificationService, INotifyHandler notifyHandler)
    {
        _userService = userService;
        _propertyService = propertyService;
        _propertyStatesService = propertyStatesService;
        _appointmentService = appointmentService;
        _notificationService = notificationService;
        _notifyHandler = notifyHandler;
    }

    public async Task<BookAppointmentResponse> Handle(BookAppointmentCommand request, CancellationToken cancellationToken)
    {
        //get user from context
        string username = _userService.FindUserNameByContext(errorTitle);
        ApplicationUser? user = await _userService.FindUserByNameAsync(username);
        if (user == null)
            throw new UserNotFoundException(username, errorTitle);

        //get owner of property
        Property property = await _propertyService.FindPropertyWithUserAndStateByIdAsync(request.PropertyId, errorTitle);

        //check if user is not same the owner
        if (user.Id == property.UserPlan.UserId)
            throw new SameOwnerException(errorTitle);

        //check if property is published
        PropertyState publishedPropertyState = await _propertyStatesService.GetPublishedPropertyStateAsync(errorTitle);
        if (property.PropertyState != publishedPropertyState)
            throw new NotPublishedPropertyException(errorTitle);

        //check if appointment still available
        DateOnly requestedDate = DateOnly.FromDateTime(request.AppointmentDate);
        List<Appointment> bookedAppointments = await _appointmentService.GetBookedAppointments(request.PropertyId, requestedDate, requestedDate, errorTitle, cancellationToken);

        if(bookedAppointments!=null && bookedAppointments.Count > 0)
        {
            bool isBlocked = bookedAppointments.Any(app =>
            {
                TimeOnly appStart = TimeOnly.FromDateTime(app.Date);
                TimeOnly appEnd = app.EndTime;

                return TimeOnly.FromDateTime(request.AppointmentDate) < appEnd && appStart < request.EndTime;
            });
            if(isBlocked)
                throw new TimeSlotNotAvailableException(errorTitle);
        }

        //get pending status
        AppointmentState pendingAppointmentSate = await _appointmentService.GetPendingAppointmentStateAsync(errorTitle);

        //book appointment
        Appointment appointment = new()
        {
            Id = Guid.NewGuid(),
            User = user,
            Property = property,
            AppointmentState = pendingAppointmentSate,
            Date = request.AppointmentDate,
            EndTime = request.EndTime,
            CreatedBy = $"{user.FirstName} {user.LastName}",
            CreatedAt = DateTime.Now
        };

        await _appointmentService.AddAppointmentAsync(appointment);

        //notify owner
        List<ApplicationUser> users = new() { property.UserPlan.User };

        List<Notification> notifications = await _notificationService.InitiateNotifications("تم طلب حجز موعد لزيارة أحد عقاراتك يرجى الاطلاع على تفاصيل الموعد في الموقع الالكتروني", users, errorTitle);

        await _notifyHandler.NotifyAsync(errorTitle);


        return new BookAppointmentResponse() { Success = true, Message = "تم طلب حجز الموعد بنجاح، انتظر موافقة صاحب العقار."};
    }
}