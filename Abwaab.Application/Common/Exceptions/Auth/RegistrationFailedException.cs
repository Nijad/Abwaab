using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Auth
{
    public class RegistrationFailedException(): Exception(ArabicErrorMessages.RegistrationFailed)
    {
        public string ErrorCode { get; } = ErrorCodes.RegistrationFailed;
        public string EnglishErrorMessage { get; } = EnglishErrorMessages.RegistrationFailed;
    }
}
