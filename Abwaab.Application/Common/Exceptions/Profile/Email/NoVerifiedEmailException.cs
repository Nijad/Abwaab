using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Email
{
    public class NoVerifiedEmailException() : Exception(ErrorMessages.NoVerifiedEmail)
    {
        public string ErrorCode { get; } = ErrorCodes.NoVerifiedEmail;
    };
}
