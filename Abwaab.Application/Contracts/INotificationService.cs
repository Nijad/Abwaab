using Abwaab.Application.Features.Notifications.Queries.GetAllNotificationWays;
using Abwaab.Domain.Entities.UserEntities;

namespace Abwaab.Application.Contracts
{
    public interface INotificationService
    {
        Task<List<GetAllWaysResponse>> GetAllNotificationWaysAsync(bool onlyCanDisable = true);
        Task InitiateNotifications(string message, IList<ApplicationUser> users, string errorTitle);
    }
}
