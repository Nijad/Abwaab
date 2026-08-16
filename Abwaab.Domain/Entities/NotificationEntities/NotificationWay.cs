namespace Abwaab.Domain.Entities.NotificationEntities
{
    public class NotificationWay : BaseEntity
    {
        public string WayName { get; set; } = null!;
        public bool CanDisable { get; set; }
        public List<UserNotificationSubscription>? NotificationSuscriptions { get; set; }
    }
}
