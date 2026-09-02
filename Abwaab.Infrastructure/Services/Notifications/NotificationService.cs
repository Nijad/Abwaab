using Abwaab.Application.Common.Enums;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Mappings;
using Abwaab.Application.Contracts;
using Abwaab.Application.Features.Notifications.Queries.GetAllNotificationWays;
using Abwaab.Application.Features.Notifications.Queries.GetWebAppNotifications;
using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.NotificationEntities;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Abwaab.Infrastructure.Services.Notifications
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

        public async Task<NotificationWay> FindNotificationWayByNameAsync(string wayName)
        {
            NotificationWay? notificationWay = await _notificationWayRepository.FindNotificationWayByNameAsync(NotificationWaysEnum.Email.ToString());

            if (notificationWay == null)
                throw new NotFoundException(nameof(NotificationWay), nameof(notificationWay.WayName), wayName, "");

            return notificationWay;
        }

        public async Task<List<Notification>> InitiateNotifications(string message, List<ApplicationUser> users, string errorTitle)
        {
            // and get thier notification-way subscriptions
            List<UserNotificationSubscription> subscriptions = new();
            foreach (var user in users)
                subscriptions.AddRange(await _notificationWayRepository.GetNotificationWaysByUserAsync(user.Id, true));

            NotificationState pendingNotificationState = await FindNotificationStateByStateNameAsync(NotificationStatesEnum.Pending.ToString(), errorTitle);

            NotificationWay email = await FindNotificationWayByNameAsync(NotificationWaysEnum.Email.ToString());
            NotificationWay sms = await FindNotificationWayByNameAsync(NotificationWaysEnum.SMS.ToString());

            //insert into notifications 
            List<Notification> notifications = new();
            foreach (var subscriptioin in subscriptions)
            {
                string identifier;
                if (subscriptioin.NotificationWay == email)
                    identifier = subscriptioin.User.Email;
                else if (subscriptioin.NotificationWay == sms)
                    identifier = subscriptioin.User.PhoneNumber;
                else
                    identifier = "";

                notifications.Add(new()
                {
                    Id = Guid.NewGuid(),
                    Identifier = identifier,
                    NotificationSubscription = subscriptioin,
                    Message = message,
                    Title = errorTitle,
                    NotificationState = pendingNotificationState,
                    CreatedAt = DateTime.Now,
                });
            }
            await _notificationWayRepository.AddNotificationsRangeAsync(notifications);
            return notifications;
        }

        public async Task<NotificationState> FindNotificationStateByStateNameAsync(string notificationStateName, string errorTitle)
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

        public async Task<NotificationState> GetPendingNotficationStateAsync(string errorTitle)
        {
            return await FindNotificationStateByStateNameAsync(NotificationStatesEnum.Pending.ToString(), errorTitle);
        }

        public async Task<NotificationState> GetSentNotficationStateAsync(string errorTitle)
        {
            return await FindNotificationStateByStateNameAsync(NotificationStatesEnum.Sent.ToString(), errorTitle);
        }

        public async Task<NotificationState> GetFailedNotficationStateAsync(string errorTitle)
        {
            return await FindNotificationStateByStateNameAsync(NotificationStatesEnum.Failed.ToString(), errorTitle);
        }

        public async Task<NotificationState> GetReadNotficationStateAsync(string errorTitle)
        {
            return await FindNotificationStateByStateNameAsync(NotificationStatesEnum.Read.ToString(), errorTitle);
        }

        public async Task<NotificationState> GetPUnreadNotficationStateAsync(string errorTitle)
        {
            return await FindNotificationStateByStateNameAsync(NotificationStatesEnum.Unread.ToString(), errorTitle);
        }

        public async Task UpdateNotificationAsync(Notification notification, CancellationToken cancellationToken = default)
        {
            await _notificationWayRepository.UpdateNotification(notification, cancellationToken);
        }

        public async Task<List<Notification>> GetPendingNotificationToSend(string errorTitle)
        {
            return await _notificationWayRepository.GetPendingNotificationToSend(await GetPendingNotficationStateAsync(errorTitle));
        }

        public async Task<List<GetUserWebAppNotificationsRespnse>> GetUserNotificationsByUserIdAsync(bool unreadOnly, Guid userId)
        {
            List<Notification> notificationList = await _notificationWayRepository.GetUserNotificationsByUserIdAsync(unreadOnly, userId);

            List<GetUserWebAppNotificationsRespnse> respnses = new();

            foreach (Notification notification in notificationList)
                respnses.Add(new()
                {
                    Title = notification.Title ?? "",
                    Message = notification.Message,
                    NotificationId = notification.Id,
                    IsRead = notification.IsRead,
                });

            return respnses;
        }
    }
}
