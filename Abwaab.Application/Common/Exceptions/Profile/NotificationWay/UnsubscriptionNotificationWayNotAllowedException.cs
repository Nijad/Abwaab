using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Profile.NotificationWay
{
    public class UnsubscriptionNotificationWayNotAllowedException(string notificationWayName, string title) : MethodNotAllowed405Exception(
            message: "",
            title: title,
            errorCode: ErrorCodes.AlreadySubscribeNotificationWay,
            returnToUser: true)
    {
        string msg = $"لا يمكنك تعطيل طريقة الإشعار '{notificationWayName}'";
        public override string Message => msg;
    };
}
