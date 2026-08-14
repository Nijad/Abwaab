using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.NotificationWay
{
    public class AlreadyUnsubscribeNotificationWayException(string notificationWayName, string title) : CusotomException(
            message: "",
            title: title,
            errorCode: ErrorCodes.AlreadySubscribeNotificationWay,
            returnToUser: true)
    {
        string msg = $"أنت غير مشترك فعلاً بطريقة الإشعارات '{notificationWayName}'";
        public override string Message => msg;
    };
}
