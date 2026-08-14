using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Auth
{
    public class RegistrationFailedException(string title) :
        CusotomException(
            message: ErrorMessages.RegistrationFailed,
            title: title,
            errorCode: ErrorCodes.RegistrationFailed,
            returnToUser: true)
    {
        public string ErrorCode { get; } = ErrorCodes.RegistrationFailed;
        public string EnglishErrorMessage { get; } = ErrorMessages.RegistrationFailed;
    }
}
