using Abwaab.Application.Features.Notifications.DTOs;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Abwaab.Infrastructure.Services.Notifications
{
    public class SmsChannel : INotificationChannel
    {
        private readonly ISmsSender _smsSender;
        private readonly ILogger<SmsChannel> _logger;

        public SmsChannel(
            ISmsSender smsSender, 
            ILogger<SmsChannel> logger)
        {
            _smsSender = smsSender;
            _logger = logger;
        }

        public bool CanHandle(NotificationDTO notification)
        {
            return notification.NotificationWayName == NotificationWaysEnum.SMS.ToString();
        }

        public async Task SendAsync(NotificationDTO notification, CancellationToken cancellationToken = default)
        {
            if (!CanHandle(notification)) 
                return;
            (notification.Success ,notification.ResponseNote) = await _smsSender.SendSmsAsync(notification.Identifier, notification.Message, "");
        }
    }
}
