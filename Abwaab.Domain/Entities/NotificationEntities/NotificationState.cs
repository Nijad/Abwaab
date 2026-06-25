namespace Abwaab.Domain.Entities.NotificationEntities
{
    public class NotificationState : BaseEntity
    {
        public string StateName { get; set; } = null!;
        public List<Notification>? Notifications { get; set; }
    }
}
