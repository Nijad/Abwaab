using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.VerificationCode
{
    public class InvalidVerificationCodeException() : Exception(ErrorMessages.InvalidVerificationCode)
    {
        public string ErrorCode { get; } = ErrorCodes.InvalidVerificationCode;
        public string EnglishErrorMessage { get; } = ErrorMessagesEn.InvalidVerificationCode;
    }
}
