using MediatR;

namespace Abwaab.Application.Features.Users.Profile.NotificationWaySubscription.Subscribe
{
    public class NotificationWaySubscriptionCommand : IRequest<NotificationWaySubscriptionResponse>
    {
        public Guid NotifiactionWayId { get; set; }
    }
}
