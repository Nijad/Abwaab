using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.NotificationWay
{
    //todo: translate
    public class AlreadySubscribeNotificationWayException(string notificationWayName) : Exception($"User already has {notificationWayName} subscription as notification way")
    {
        public string ErrorCode { get; } = ErrorCodes.AlreadySubscribeNotificationWay;
    };
}
