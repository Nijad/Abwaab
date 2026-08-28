using Abwaab.Application.Features.Notifications.DTOs;
using Abwaab.Application.Features.Notifications.Queries.GetAllNotificationWays;
using Abwaab.Domain.Entities.NotificationEntities;
using Abwaab.Domain.Entities.UserEntities;

namespace Abwaab.Application.Contracts
{
    public interface INotificationService
    {
        Task<List<GetAllWaysResponse>> GetAllNotificationWaysAsync(bool onlyCanDisable = true);
        Task<List<Notification>> InitiateNotifications(string message, List<ApplicationUser> users, string errorTitle);
        Task<NotificationWay> FindNotificationWayByNameAsync(string wayName);
        Task<NotificationState> FindNotificationStateByStateNameAsync(string notificationStateName, string errorTitle);
        Task<NotificationState> GetPendingNotficationStateAsync(string errorTitle);
        Task<NotificationState> GetSentNotficationStateAsync(string errorTitle);
        Task<NotificationState> GetFailedNotficationStateAsync(string errorTitle);
        Task<NotificationState> GetReadNotficationStateAsync(string errorTitle);
        Task<NotificationState> GetPUnreadNotficationStateAsync(string errorTitle);
        Task UpdateNotificationAsync(Notification notification, CancellationToken cancellationToken = default);
        Task<List<Notification>> GetPendingNotificationToSend(string errorTitle);
    }
}
