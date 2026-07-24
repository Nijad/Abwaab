using Abwaab.Domain.Entities.NotificationEntities;

namespace Abwaab.Application.Common.Interfaces
{
    public interface INotificationWayRepository
    {
        Task<IEnumerable<NotificationWay>> GetNotificationWays();
        Task<NotificationWay?> GetNotificationWay(string wayName);
        Task<List<UserNotificationSubscription>> GetUserNotificationWays(Guid userId);
    }
}
