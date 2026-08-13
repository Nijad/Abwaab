using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Email
{
    public class NoVerifiedEmailException() : Exception(ArabicErrorMessages.NoVerifiedEmail)
    {
        public string ErrorCode { get; } = ErrorCodes.NoVerifiedEmail;
        public string EnglishErrorMessage { get; } = EnglishErrorMessages.NoVerifiedEmail;
    };
}
