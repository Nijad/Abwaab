using Abwaab.Application.Common.Enums;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Mappings;
using Abwaab.Application.Contracts;
using Abwaab.Application.Features.Notifications.Queries.GetAllNotificationWays;
using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.NotificationEntities;
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Domain.Entities.UserEntities;
using Microsoft.Extensions.Logging;

namespace Abwaab.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationWayRepository _notificationWayRepository;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            INotificationWayRepository notificationWayRepository,
            ILogger<NotificationService> logger)
        {
            _notificationWayRepository = notificationWayRepository;
            _logger = logger;
        }

        public async Task<List<GetAllWaysResponse>> GetAllNotificationWaysAsync(bool onlyCanDisable = true)
        {
            var ways = await _notificationWayRepository.GetAllNotificationWaysAsync(onlyCanDisable);

            List<GetAllWaysResponse> responses = new();
            foreach (var way in ways)
                responses.Add(new() { Id = way.Id, WayName = NotificationWaysMapping.Map(way.WayName) });
            return responses;
        }

        public async Task InitiateNotifications(string message, IList<ApplicationUser> users, string errorTitle)
        {
            // and get thier notification-way subscriptions
            List<UserNotificationSubscription> subscriptions = new();
            foreach (var admin in users)
                subscriptions.AddRange(await _notificationWayRepository.GetNotificationWaysByUserAsync(admin.Id, true));

            NotificationState pendingNotificationState = await FindNotificationStateByStateNameAsync(NotificationStatesEnum.Pending.ToString(), errorTitle);

            //insert into notifications 
            List<Notification> notifications = new();
            foreach (var subscriptioin in subscriptions)
                notifications.Add(new()
                {
                    Id = Guid.NewGuid(),
                    NotificationSubscription = subscriptioin,
                    Message = message,
                    Title = errorTitle,
                    NotificationState = pendingNotificationState,
                    CreatedAt = DateTime.Now,
                });

            await _notificationWayRepository.AddNotificationsRangeAsync(notifications);
        }

        private async Task<NotificationState> FindNotificationStateByStateNameAsync(string notificationStateName, string errorTitle)
        { 
            NotificationState? state = await _notificationWayRepository.FindNotificationStateByStateNameAsync(notificationStateName);

            if (state == null)
                throw new NotFoundException(
                    nameof(NotificationState),
                    nameof(state.StateName),
                    notificationStateName,
                    errorTitle);

            return state;
        }
    }
}
