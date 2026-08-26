using Abwaab.Domain.Entities.NotificationEntities;

namespace Abwaab.Application.Repositories
{
    public interface INotificationWayRepository
    {
        Task AddSubscriptionAsync(UserNotificationSubscription userSubscription);
        Task<List<UserNotificationSubscription>> GetAllNotificationWaysOfUserAsync(Guid userId);
        Task<IEnumerable<NotificationWay>> GetAllNotificationWaysAsync(bool onlyCanDisable = true);
        
        Task<NotificationWay?> GetNotificationWayByIdAsync(Guid id);
        
        Task<NotificationWay?> GetNotificationWayByNameAsync(string wayName);
        
        Task<List<UserNotificationSubscription>> GetNotificationWaysByUserAsync(Guid userId, bool activeOnly = false);
        
        Task<UserNotificationSubscription?> GetUserSubscriptionAsync(Guid userId, Guid notifiactionWayId);
        Task<bool> HasUserActiveNotificationWay(Guid userId, Guid notifiacationWayId);
        Task UpdateSubscriptionAsync(UserNotificationSubscription userSubscription);
        Task<NotificationState?> FindNotificationStateByStateNameAsync(string notificationStateName);
        Task AddNotificationsRangeAsync(List<Notification> notifications);
    }
}
