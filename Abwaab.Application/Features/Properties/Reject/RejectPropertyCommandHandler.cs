namespace Abwaab.Application.Features.Properties.Reject;

using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Common.Exceptions.Properties.States;
using Abwaab.Application.Contracts;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.NotificationEntities;
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;

public class RejectPropertyCommandHandler : IRequestHandler<RejectPropertyCommand, RejectPropertyResponse>
{
    private readonly IPropertyStatesService _propertyStatesService;
    private readonly IPropertyService _propertyService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly INotificationService _notificationService;
    private readonly INotifyHandler _notifyHandler;
    private readonly string errorTitle = ErrorTitle.RejectProperty;

    public RejectPropertyCommandHandler(IPropertyStatesService propertyStatesService, IPropertyService propertyService, UserManager<ApplicationUser> userManager, INotificationService notificationService, INotifyHandler notifyHandler)
    {
        _propertyStatesService = propertyStatesService;
        _propertyService = propertyService;
        _userManager = userManager;
        _notificationService = notificationService;
        _notifyHandler = notifyHandler;
    }

    public async Task<RejectPropertyResponse> Handle(RejectPropertyCommand request, CancellationToken cancellationToken)
    {
        //get property
        Property property = await _propertyService.FindPropertyWithUserAndStateByIdAsync(request.PropertyId, errorTitle);

        //check if current state is pending
        PropertyState pendingPropertyState = await _propertyStatesService.GetPendingPropertyStateAsync(errorTitle);

        if (property.PropertyState != pendingPropertyState)
            throw new NotAllowedToSetPropertyAsRejectedException(property.PropertyState.StateName, errorTitle);

        //update property state
        PropertyState rejectedPropertyState = await _propertyStatesService.GetRejectedPropertyStateAsync(errorTitle);

        property.PropertyState = rejectedPropertyState;
        property.Note = request.Note;
        await _propertyService.UpdatePropertyAsync(property);

        //preparing send notification to the owner
        string userId = property.UserPlan.UserId.ToString();
        ApplicationUser? user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            throw new UserNotFoundException(userId ,errorTitle);

        List<ApplicationUser> users = new(){ user };

        List<Notification> notifications = await _notificationService.InitiateNotifications("تم رفض العقار الخاص بك من قبل إدارة الموقع. يمكنك الاطلاع على التفاصيل من خلال الموقع الالكتروني", users, errorTitle);

        await _notifyHandler.NotifyAsync(errorTitle);
        return new RejectPropertyResponse() { Success = true , Message = "تم رفض العقار بنجاح."};
    }
}
