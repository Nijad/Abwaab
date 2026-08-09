using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Auth
{
    public class RegistrationFailedException(): Exception(ErrorMessages.RegistrationFailed)
    {
        public string ErrorCode { get; } = ErrorCodes.RegistrationFailed;
    }
}
