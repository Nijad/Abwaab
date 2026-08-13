using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.VerificationCode
{
    public class InvalidVerificationCodeException() : Exception(ArabicErrorMessages.InvalidVerificationCode)
    {
        public string ErrorCode { get; } = ErrorCodes.InvalidVerificationCode;
        public string EnglishErrorMessage { get; } = EnglishErrorMessages.InvalidVerificationCode;
    }
}
