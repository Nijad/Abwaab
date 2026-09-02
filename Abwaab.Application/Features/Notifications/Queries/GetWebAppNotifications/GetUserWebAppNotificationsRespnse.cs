namespace Abwaab.Application.Features.Notifications.Queries.GetWebAppNotifications;

public class GetUserWebAppNotificationsRespnse
{
    public Guid NotificationId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
}
