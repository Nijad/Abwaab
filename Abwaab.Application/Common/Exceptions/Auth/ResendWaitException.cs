using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Auth
{
    public class ResendWaitException() : Exception($"Please wait {GeneralConstants.WAIT_TIMEOUT_MINUTES * 60} seconds before requesting a new verification code.")
    {
        public string ErrorCode { get; } = ErrorCodes.ResendWait;
    };
}
