using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Common.Exceptions.Properties.States;
using Abwaab.Application.Contracts;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Features.Properties.Reject;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.NotificationEntities;
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Abwaab.Application.Features.Properties.Accept;

public class DesablePropertyCommandHandler : IRequestHandler<DisablePropertyCommand, DisablePropertyResponse>
{
    private readonly IPropertyStatesService _propertyStatesService;
    private readonly IPropertyService _propertyService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly INotificationService _notificationService;
    private readonly INotifyHandler _notifyHandler;
    private readonly string errorTitle = ErrorTitle.AcceptProperty;

    public DesablePropertyCommandHandler(
        IPropertyStatesService propertyStatesService,
        IPropertyService propertyService,
        UserManager<ApplicationUser> userManager,
        INotificationService notificationService,
        INotifyHandler notifyHandler)
    {
        _propertyStatesService = propertyStatesService;
        _propertyService = propertyService;
        _userManager = userManager;
        _notificationService = notificationService;
        _notifyHandler = notifyHandler;
    }

    public async Task<DisablePropertyResponse> Handle(DisablePropertyCommand request, CancellationToken cancellationToken)
    {
        //get property
        Property property = await _propertyService.FindPropertyWithUserAndStateByIdAsync(request.PropertyId, errorTitle);

        //check if current state is pending
        PropertyState pendingPropertyState = await _propertyStatesService.GetPendingPropertyStateAsync(errorTitle);

        if (property.PropertyState != pendingPropertyState)
            throw new NotAllowedToSetPropertyAsPublishedException(property.PropertyState.StateName, errorTitle);

        //update property state
        PropertyState publishedPropertyState = await _propertyStatesService.GetPublishedPropertyStateAsync(errorTitle);

        property.PropertyState = publishedPropertyState;
        property.Note = "";
        await _propertyService.UpdatePropertyAsync(property);

        //preparing send notification to the owner
        string userId = property.UserPlan.UserId.ToString();
        ApplicationUser? user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            throw new UserNotFoundException(userId, errorTitle);

        List<ApplicationUser> users = new() { user };

        List<Notification> notifications = await _notificationService.InitiateNotifications("تم نشر العقار الخاص بك، وأصبح متاحاً للاستعراض على الموقع الالكتروني.", users, errorTitle);

        await _notifyHandler.NotifyAsync(errorTitle);
        return new DisablePropertyResponse() { Success = true, Message = "تم رفض العقار بنجاح." };
    }
}
