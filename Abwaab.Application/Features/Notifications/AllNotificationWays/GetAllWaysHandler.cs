using Abwaab.Application.Contracts;
using MediatR;

namespace Abwaab.Application.Features.Notifications.AllNotificationWays
{
    public class GetAllWaysHandler : IRequestHandler<GetAllWaysQuery, List<GetAllWaysResponse>>
    {
        private readonly INotificationService _notificationService;

        public GetAllWaysHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<List<GetAllWaysResponse>> Handle(GetAllWaysQuery request, CancellationToken cancellationToken)
        {
            return await _notificationService.GetAllNotificationWaysAsync(false);
        }
    }
}
