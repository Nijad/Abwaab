using MediatR;

namespace Abwaab.Application.DTOs.Profile.NotificationWayUnsubscription
{
    public class NotificationWaySubsciptionCommand : IRequest<NotificationWayUnsubscriptionResponse>
    {
        public Guid UserId { get; set; }
        public Guid NotifiactionWayId { get; set; }
    }
}
