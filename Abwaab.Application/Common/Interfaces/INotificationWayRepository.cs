using Abwaab.Domain.Entities.NotificationEntities;

namespace Abwaab.Application.Common.Interfaces
{
    public interface INotificationWayRepository
    {
        Task AddSubscriptionAsync(UserNotificationSubscription userSubscription);
        Task<IEnumerable<NotificationWay>> GetNotificationAllWaysAsync();
        
        Task<NotificationWay?> GetNotificationWayByIdAsync(Guid id);
        
        Task<NotificationWay?> GetNotificationWayByNameAsync(string wayName);
        
        Task<List<UserNotificationSubscription>> GetNotificationWaysByUserAsync(Guid userId);
        
        Task<UserNotificationSubscription?> GetUserSubscriptionAsync(Guid userId, Guid notifiactionWayId);
        Task UpdateSubscriptionAsync(UserNotificationSubscription userSubscription);
    }
}
