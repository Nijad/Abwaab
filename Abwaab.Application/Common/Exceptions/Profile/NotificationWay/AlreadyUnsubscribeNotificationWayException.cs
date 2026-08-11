using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.NotificationWay
{
    //todo: translate
    public class AlreadyUnsubscribeNotificationWayException(string notificationWayName) : Exception($"User already unsubscribe {notificationWayName} as notification way")
    {
        public string ErrorCode { get; } = ErrorCodes.AlreadySubscribeNotificationWay;
    };
}
