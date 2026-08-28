namespace Abwaab.Domain.Entities.NotificationEntities
{
    public class Notification : BaseEntity
    {
        public string Identifier { get; set; } = null!;
        public string? Title { get; set; }
        public string Message { get; set; } = null!;
        public bool IsRead { get; set; }
        public UserNotificationSubscription NotificationSubscription { get; set; } = null!;
        public Guid NotificationSubscriptionId { get; set; }
        public NotificationState NotificationState { get; set; } = null!;
        public Guid NotificationStateId { get; set; }
        public string? ResponseNote { get; set; }
    }
}