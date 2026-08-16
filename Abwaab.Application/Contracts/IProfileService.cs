using Abwaab.Application.Features.Users.Profile.NotificationWaySubscription.Subscribe;
using Abwaab.Application.Features.Users.Profile.NotificationWaySubscription.Unsubscribe;
using Abwaab.Domain.Entities.NotificationEntities;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Domain.Enums;

namespace Abwaab.Application.Contracts
{
    public interface IProfileService
    {
        Task<NotificationWaySubscriptionResponse> SubscribeNotificationWayCommandAsync(ApplicationUser user, Guid notificationWayId, string errorTitle);
        Task<NotificationWayUnsubscriptionResponse> UnsubscribeNotificationWayCommandAsync(ApplicationUser user, Guid notificationWayId, string errorTitle);
        Task SubscribeNotificationWayCommandAsync(ApplicationUser user, NotificationWaysEnum notificationWayType);
        Task RevokeAllRefreshToken(Guid userId, string reason);
        Task<List<UserNotificationSubscription>> GetAllUserNotificationWaysAsync(Guid userId);
        Task<NotificationWay> FindNotificationWayByNameAsync(NotificationWaysEnum wayName, string errorTitle);
        Task<bool> HasUserActiveNotificationWay(Guid userId, Guid notifiacationWayId, string errorTitle);
        Task<bool> HasActivatedEmailNotificationWay(Guid id, string errorTitle);
        Task<bool> HasActivatedSmsNotificationWay(Guid id, string errorTitle);
        Task<bool> HasActivatedWebNotificationWay(Guid id, string errorTitle);
    }
}
