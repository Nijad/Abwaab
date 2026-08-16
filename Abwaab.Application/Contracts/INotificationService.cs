using Abwaab.Application.Features.Notifications.AllNotificationWays;

namespace Abwaab.Application.Contracts
{
    public interface INotificationService
    {
        Task<List<GetAllWaysResponse>> GetAllNotificationWaysAsync();
    }
}
