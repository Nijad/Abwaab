using MediatR;

namespace Abwaab.Application.Features.Notifications.Queries.GetAllNotificationWays
{
    public class GetAllWaysQuery : IRequest<List<GetAllWaysResponse>>
    {
    }
}
