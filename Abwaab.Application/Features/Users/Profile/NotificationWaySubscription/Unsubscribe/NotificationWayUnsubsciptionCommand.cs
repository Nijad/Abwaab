using MediatR;

namespace Abwaab.Application.Features.Users.Profile.NotificationWaySubscription.Unsubscribe
{
    //todo: no need user id
    public class NotificationWayUnsubsciptionCommand : IRequest<NotificationWayUnsubscriptionResponse>
    {
        public Guid UserId { get; set; }
        public Guid NotifiactionWayId { get; set; }
    }
}
