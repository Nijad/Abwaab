using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.VerificationCode
{
    public class InvalidVerificationCodeException() : Exception()
    {
        public string ErrorCode { get; } = ErrorCodes.InvalidVerificationCode;
    }
}
