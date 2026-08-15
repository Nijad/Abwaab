using MediatR;

namespace Abwaab.Application.Features.Users.Profile.NotificationWaySubscription.Subscribe
{
    //todo: no need user id, must get it from context
    public class NotificationWaySubscriptionCommand : IRequest<NotificationWaySubscriptionResponse>
    {
        public Guid UserId { get; set; }
        public Guid NotifiactionWayId { get; set; }
    }
}
