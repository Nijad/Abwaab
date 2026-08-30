using Abwaab.Application.Contracts;
using Abwaab.Application.Features.Notifications.DTOs;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.NotificationEntities;
using Microsoft.Extensions.Logging;

namespace Abwaab.Application.Features.Notify;

public class NotifyHandler : INotifyHandler
{
    private readonly INotificationService _notificationService;
    private readonly IEnumerable<INotificationChannel> _channels;
    private readonly ILogger<NotifyHandler> _logger;

    public NotifyHandler(
        INotificationService notificationService,
        IEnumerable<INotificationChannel> channels,
        ILogger<NotifyHandler> logger)
    {
        _notificationService = notificationService;
        _channels = channels;
        _logger = logger;
    }

    public async Task NotifyAsync(string errorTitle)
    {
        List<Notification> pendingNotificationsToSend = await _notificationService.GetPendingNotificationToSend(errorTitle);

        foreach (var notification in pendingNotificationsToSend)
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
                await channel.SendAsync(notificationDTO);
                if (notificationDTO.Success)
                    notification.NotificationState = await _notificationService.GetSentNotficationStateAsync(errorTitle);
                else
                    notification.NotificationState = await _notificationService.GetFailedNotficationStateAsync(errorTitle);
            }
            catch (Exception ex)
            {
                notification.NotificationState = await _notificationService.GetFailedNotficationStateAsync(errorTitle);

                _logger.LogError(ex, "error while sending notification");
            }
            notification.ResponseNote = notificationDTO.ResponseNote;

            await _notificationService.UpdateNotificationAsync(notification);
        }
    }
}
