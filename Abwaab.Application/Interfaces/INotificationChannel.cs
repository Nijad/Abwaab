using Abwaab.Application.Features.Notifications.DTOs;

namespace Abwaab.Application.Interfaces
{
    public interface INotificationChannel
    {
        bool CanHandle(NotificationDTO notification);
        Task SendAsync(NotificationDTO notification, CancellationToken cancellationToken = default);
    }
}
