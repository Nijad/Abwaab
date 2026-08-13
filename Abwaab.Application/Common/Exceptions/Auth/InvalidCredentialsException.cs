using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Auth
{
    public class InvalidCredentialsException() : Exception(ArabicErrorMessages.InvalidCredentials)
    {
        public string ErrorCode { get; } = ErrorCodes.InvalidCredentials;
        public string EnglishErrorMessage { get; } = EnglishErrorMessages.InvalidCredentials;
    };
}
