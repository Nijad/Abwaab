using Abwaab.Application.Common.Mappings;
using Abwaab.Application.Contracts;
using Abwaab.Application.Features.Notifications.Queries.GetAllNotificationWays;
using Abwaab.Application.Repositories;

namespace Abwaab.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationWayRepository _notificationWayRepository;

        public NotificationService(INotificationWayRepository notificationWayRepository)
        {
            _notificationWayRepository = notificationWayRepository;
        }

        public async Task<List<GetAllWaysResponse>> GetAllNotificationWaysAsync(bool onlyCanDisable = true)
        {
            var ways =  await _notificationWayRepository.GetAllNotificationWaysAsync(onlyCanDisable);

            List<GetAllWaysResponse> responses = new();
            foreach (var way in ways)
                responses.Add(new() { Id = way.Id , WayName = NotificationWaysMapping.Map(way.WayName)});
            return responses;
        }
    }
}
