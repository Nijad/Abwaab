namespace Abwaab.Application.Features.Notifications.DTOs
{
    public class NotificationDTO
    {
        public Guid NotificationId { get; set; }
        public string? Title { get; set; }
        public string Message { get; set; } = null!;
        public string? ResponseNote { get; set; }
        public string NotificationWayName { get; set; } = null!;
        public string Identifier { get; set; } = null!;
        public bool Success { get; set; }
    }
}
