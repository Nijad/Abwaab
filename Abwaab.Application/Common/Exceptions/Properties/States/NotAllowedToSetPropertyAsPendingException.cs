using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;
namespace Abwaab.Application.Common.Exceptions.Properties.States
{
    public class NotAllowedToSetPropertyAsPendingException(string stateName, string title) :
            MethodNotAllowed405Exception(
                message: "",
                title: title,
                errorCode: ErrorCodes.NotAllowedToSetPropertyAsPending,
                returnToUser: true)
    {
        string msg = $"لا يمكنك وضع العقار بحالة انتظار موافقة الإدارة، حيث أن حالة العقار الحالية هي '{stateName}'.";
        public override string Message => msg;
    }
}
