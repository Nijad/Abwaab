using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Auth
{
    public class ResendWaitException(string title): PreconditionRequired428Exception(
        message: "",
            title: title,
            errorCode: ErrorCodes.ResendWait,
            returnToUser: true)
    {
        string msg = $"الرجاء الانتظار لمدة  {GeneralConstants.WAIT_TIMEOUT_MINUTES * 60} ثانية قبل إعادة طلب رمز تفعيل جديد.";
        public override string Message => msg;
    }
}
