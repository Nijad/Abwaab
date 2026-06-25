namespace Abwaab.Domain.Entities.NotificationEntities
{
    public class NotificationWay : BaseEntity
    {
        public string WayName { get; set; } = null!;
        public List<UserNotificationSubscription>? NotificationSuscriptions { get; set; }
    }
}
