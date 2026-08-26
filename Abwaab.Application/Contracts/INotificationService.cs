using Abwaab.Application.Features.Notifications.Queries.GetAllNotificationWays;

namespace Abwaab.Application.Contracts
{
    public interface INotificationService
    {
        Task<List<GetAllWaysResponse>> GetAllNotificationWaysAsync(bool onlyCanDisable = true);
    }
}
