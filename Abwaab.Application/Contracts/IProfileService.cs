using Abwaab.Application.Features.Users.Profile.NotificationWaySubscription.Subscribe;
using Abwaab.Application.Features.Users.Profile.NotificationWaySubscription.Unsubscribe;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Domain.Enums;

namespace Abwaab.Application.Contracts
{
    public interface IProfileService
    {
        Task<NotificationWaySubscriptionResponse> SubscribeNotificationWayCommandAsync(ApplicationUser user, Guid notificationWayId, string errorTitle);
        Task<NotificationWayUnsubscriptionResponse> UnsubscribeNotificationWayCommandAsync(ApplicationUser user, Guid notificationWayId, string errorTitle);
        Task<bool> SubscribeNotificationWayCommandAsync(ApplicationUser user, NotificationWaysEnum notificationWayType);
        Task RevokeAllRefreshToken(Guid userId, string reason);
    }
}
