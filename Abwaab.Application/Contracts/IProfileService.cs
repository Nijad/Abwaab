using Abwaab.Application.Features.Users.Profile.NotificationWaySubscription.Subscribe;
using Abwaab.Application.Features.Users.Profile.NotificationWaySubscription.Unsubscribe;
using Abwaab.Application.Features.Users.Profile.Phone.InitiateChange;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Domain.Enums;

namespace Abwaab.Application.Contracts
{
    public interface IProfileService
    {
        Task<NotificationWaySubscriptionResponse> SubscribeNotificationWayCommandAsync(ApplicationUser user, Guid notificationWayId);
        Task<NotificationWayUnsubscriptionResponse> UnsubscribeNotificationWayCommandAsync(ApplicationUser user, Guid notificationWayId);
        Task<bool> SubscribeNotificationWayCommandAsync(ApplicationUser user, NotificationWayEnum notificationWayType);
        Task RevokeAllRefreshToken(Guid userId, string reason);
    }
}
