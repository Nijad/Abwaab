using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Auth
{
    public class InvalidCredentialsException() : Exception(ErrorMessages.InvalidCredentials)
    {
        public string ErrorCode { get; } = ErrorCodes.InvalidCredentials;
    };
}
