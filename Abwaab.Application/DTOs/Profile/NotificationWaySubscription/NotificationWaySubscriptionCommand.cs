using MediatR;

namespace Abwaab.Application.DTOs.Profile.NotificationWaySubscription
{
    public class NotificationWaySubscriptionCommand : IRequest<NotificationWaySubscriptionResponse>
    {
        public Guid UserId { get; set; }
        public Guid NotifiactionWayId { get; set; }
    }
}
