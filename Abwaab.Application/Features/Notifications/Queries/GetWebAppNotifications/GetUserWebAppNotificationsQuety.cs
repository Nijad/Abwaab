using MediatR;

namespace Abwaab.Application.Features.Notifications.Queries.GetWebAppNotifications;

public class GetUserWebAppNotificationsQuety : IRequest<List<GetUserWebAppNotificationsRespnse>>
{
    public bool UnreadOnly { get; set; }
}
