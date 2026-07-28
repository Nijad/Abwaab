using Abwaab.Application.DTOs.Profile.NotificationWaySubscription;
using Abwaab.Application.DTOs.Profile.NotificationWayUnsubscription;

namespace Abwaab.Application.Common.Contracts
{
    public interface IProfileService
    {
        Task<NotificationWaySubscriptionResponse> SubscribeNotificationWayCommandAsync(NotificationWaySubscriptionCommand request);
        Task<NotificationWayUnsubscriptionResponse> UnsubscribeNotificationWayCommandAsync(NotificationWaySubsciptionCommand request);
    }
}
