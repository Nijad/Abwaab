using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Auth
{
    public class InvalidCredentialsException() : Exception(ErrorMessages.InvalidCredentials)
    {
        public string ErrorCode { get; } = ErrorCodes.InvalidCredentials;
        public string EnglishErrorMessage { get; } = ErrorMessagesEn.InvalidCredentials;
    };
}
