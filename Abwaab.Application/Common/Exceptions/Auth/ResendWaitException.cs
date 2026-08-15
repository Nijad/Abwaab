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
        string msg = $"Please wait {GeneralConstants.WAIT_TIMEOUT_MINUTES * 60} seconds before requesting a new verification code.";
        public override string Message => msg;
    }
}
