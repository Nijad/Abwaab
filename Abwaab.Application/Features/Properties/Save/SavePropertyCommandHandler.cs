using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Common.Exceptions.Media;
using Abwaab.Application.Common.Exceptions.Properties.States;
using Abwaab.Application.Contracts;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Features.Notifications.DTOs;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.NotificationEntities;
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Abwaab.Application.Features.Properties.Save
{
    public class SavePropertyCommandHandler : IRequestHandler<SavePropertyCommand, SavePropertyResponse>
    {
        private readonly IUserService _userService;
        private readonly IPropertyStatesService _propertyStatesService;
        private readonly IPropertyService _propertyService;
        private readonly IMediaService _mediaService;
        private readonly INotificationService _notificationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEnumerable<INotificationChannel> _channels;
        private readonly string errorTitle = ErrorTitle.SaveProperty;

        public SavePropertyCommandHandler(
            IUserService userService,
            IPropertyStatesService propertyStatesService,
            IPropertyService propertyService,
            IMediaService mediaService,
            INotificationService notificationService,
            UserManager<ApplicationUser> userManager,
            IEnumerable<INotificationChannel> channels)
        {
            _userService = userService;
            _propertyStatesService = propertyStatesService;
            _propertyService = propertyService;
            _mediaService = mediaService;
            _notificationService = notificationService;
            _userManager = userManager;
            _channels = channels;
        }

        public async Task<SavePropertyResponse> Handle(SavePropertyCommand request, CancellationToken cancellationToken)
        {
            //check if property exist
            Property property = await _propertyService.FindPropertyByIdAsync(request.PropertyId, errorTitle);


            //check if property belong to user
            string username = _userService.FindUserNameByContext(errorTitle);
            ApplicationUser? user = await _userService.FindUserByNameAsync(username);

            if (user == null)
                throw new UserNotFoundException(username, errorTitle);

            //check if property belong to user
            if (property.UserPlan.UserId != user.Id)
                throw new ObjectNotBelongToUserException("العقار", errorTitle);

            //check if has cover image
            bool hasCover = await _mediaService.HasPropertyCoverAsync(property.Id);
            if (!hasCover)
                throw new HasNoCoverImageException(errorTitle);

            //check property state if is preparing
            PropertyState preparingState = await _propertyStatesService.GetPreparingPropertyStateAsync(errorTitle);

            if (property.PropertyStateId != preparingState.Id)
                throw new NotAllowedToSetPropertyAsPendingException(property.PropertyState.StateName, errorTitle);

            //save property (change state to pending)
            PropertyState pendingState = await _propertyStatesService.GetPendingPropertyStateAsync(errorTitle);
            property.PropertyState = pendingState;
            await _propertyService.UpdatePropertyAsync(property);

            try
            {
                //push notification
                IList<ApplicationUser> admins = await _userManager.GetUsersInRoleAsync(RoleConstants.ROLE_ADMIN);

                List<Notification> notifications = await _notificationService.InitiateNotifications("هناك عقاراً جديداً بانتظار الموافقة", admins, errorTitle);

                foreach (var notification in notifications)
                {
                    NotificationDTO notificationDTO = new()
                    {
                        NotificationId = notification.Id,
                        Identifier = notification.Identifier,
                        Message = notification.Message,
                        Title = notification.Title,
                        ResponseNote = notification.ResponseNote,
                        NotificationWayName = notification.NotificationSubscription.NotificationWay.WayName
                    };

                    INotificationChannel? channel = _channels.FirstOrDefault(c => c.CanHandle(notificationDTO));

                    if (channel == null)
                        notification.NotificationState = await _notificationService.GetPUnreadNotficationStateAsync(errorTitle);
                    try
                    {
                        await channel.SendAsync(notificationDTO, cancellationToken);
                        if (notificationDTO.Success)
                            notification.NotificationState = await _notificationService.GetSentNotficationStateAsync(errorTitle);
                        else
                            notification.NotificationState = await _notificationService.GetFailedNotficationStateAsync(errorTitle);
                    }
                    catch(Exception ex)
                    {
                        notification.NotificationState = await _notificationService.GetFailedNotficationStateAsync(errorTitle);
                        //todo : log exception here
                    }
                    notification.ResponseNote = notificationDTO.ResponseNote;

                    await _notificationService.UpdateNotificationAsync(notification, cancellationToken);
                }
            }
            catch
            {
                //log error here
            }


            return new SavePropertyResponse() { Success = true, Message = "تم حفظ العقار بنجاح وهو الآن قيد انتظار موافقة الإدارة، سيتم إعلامكم بذلك في غضون 48 ساعة كحد أقصى." };
        }
    }
}
