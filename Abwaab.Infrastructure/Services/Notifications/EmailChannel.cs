using Abwaab.Application.Features.Notifications.DTOs;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Abwaab.Infrastructure.Services.Notifications
{
    public class EmailChannel : INotificationChannel
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<EmailChannel> _logger;

        public EmailChannel(
            IEmailSender emailSender, 
            ILogger<EmailChannel> logger)
        {
            _emailSender = emailSender;
            _logger = logger;
        }


        public bool CanHandle(NotificationDTO notification)
        {
            return notification.NotificationWayName == NotificationWaysEnum.Email.ToString();
        }

        public async Task SendAsync(NotificationDTO notification, CancellationToken cancellationToken = default)
        {
            if (!CanHandle(notification))
                return;

            var subject = notification.Title;
            var body = notification.Message;

            if (string.IsNullOrEmpty(notification.Title))
                subject = "Notification"; 

            (notification.Success ,notification.ResponseNote) = await _emailSender.SendEmailAsync(notification.Identifier, subject, body, "");

        }
    }
}
