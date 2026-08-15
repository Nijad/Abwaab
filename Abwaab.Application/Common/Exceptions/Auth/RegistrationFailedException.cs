using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Auth
{
    public class RegistrationFailedException(string title) :
        BadRequest400Exception(
            message: ErrorMessages.RegistrationFailed,
            title: title,
            errorCode: ErrorCodes.RegistrationFailed,
            returnToUser: true)
    {
        public string ErrorCode { get; } = ErrorCodes.RegistrationFailed;
        public string EnglishErrorMessage { get; } = ErrorMessages.RegistrationFailed;
    }
}
