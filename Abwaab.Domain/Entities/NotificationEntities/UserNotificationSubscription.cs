using Abwaab.Domain.Entities.UserEntities;

namespace Abwaab.Domain.Entities.NotificationEntities
{
    public class UserNotificationSubscription : BaseEntity
    {
        public ApplicationUser User { get; set; } = null!;
        public Guid UserId { get; set; }
        public NotificationWay NotificationWay { get; set; } = null!;
        public Guid NotificationWayId { get; set; }
        public bool IsInactive { get; set; }
        public List<Notification>? Notifications { get; set; }
    }
}
