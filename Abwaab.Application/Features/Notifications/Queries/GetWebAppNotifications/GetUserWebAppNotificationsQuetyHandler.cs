using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Contracts;
using Abwaab.Domain.Entities.NotificationEntities;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Domain.Enums;
using MediatR;

namespace Abwaab.Application.Features.Notifications.Queries.GetWebAppNotifications;

public class GetUserWebAppNotificationsQuetyHandler : IRequestHandler<GetUserWebAppNotificationsQuety, List<GetUserWebAppNotificationsRespnse>>
{
    private readonly IUserService _userService;
    private readonly INotificationService _notificationService;
    

    private readonly string errorTitle = ErrorTitle.GetUserNotifications;

    public GetUserWebAppNotificationsQuetyHandler(IUserService userService, INotificationService notificationService)
    {
        _userService = userService;
        _notificationService = notificationService;
    }

    public async Task<List<GetUserWebAppNotificationsRespnse>> Handle(GetUserWebAppNotificationsQuety request, CancellationToken cancellationToken)
    {
        string username = _userService.FindUserNameByContext(errorTitle);
        ApplicationUser? user = await _userService.FindUserByNameAsync(username);
        if (user == null)
            throw new UserNotFoundException(username, errorTitle);

        NotificationWay notificationWay = await _notificationService.GetNotificationWayByNameAsync(NotificationWaysEnum.Web_Application, errorTitle);

        List<GetUserWebAppNotificationsRespnse> notifications =await _notificationService.GetWebUserNotificationsByUserIdAsync(request.UnreadOnly ,user.Id, notificationWay);

        return notifications;
    }
}