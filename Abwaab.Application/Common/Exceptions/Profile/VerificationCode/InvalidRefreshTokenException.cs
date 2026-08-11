using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.VerificationCode
{
    public class InvalidRefreshTokenException() : Exception(ErrorMessages.InvalidRefreshToken)
    {
        public string ErrorCode { get; } = ErrorCodes.InvalidRefreshToken;
        public string EnglishErrorMessage { get; } = ErrorMessagesEn.InvalidRefreshToken;
    }
}
