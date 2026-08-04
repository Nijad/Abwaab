using MediatR;

namespace Abwaab.Application.Features.Users.Profile.NotificationWaySubscription.Unsubscribe
{
    public class NotificationWayUnsubsciptionCommand : IRequest<NotificationWayUnsubscriptionResponse>
    {
        public Guid UserId { get; set; }
        public Guid NotifiactionWayId { get; set; }
    }
}
