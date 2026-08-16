using MediatR;

namespace Abwaab.Application.Features.Notifications.AllNotificationWays
{
    public class GetAllWaysQuery : IRequest<List<GetAllWaysResponse>>
    {
    }
}
