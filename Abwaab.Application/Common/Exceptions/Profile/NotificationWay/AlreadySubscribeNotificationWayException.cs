using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Profile.NotificationWay
{
    public class AlreadySubscribeNotificationWayException(string notificationWayName, string title) : BadRequest400Exception(
            message: "",
            title: title,
            errorCode: ErrorCodes.AlreadySubscribeNotificationWay,
            returnToUser: true)
    {
        string msg = $"أنت مشترك فعلاً بطريقة الإشعارات '{notificationWayName}'";
        public override string Message => msg;
    };
}
