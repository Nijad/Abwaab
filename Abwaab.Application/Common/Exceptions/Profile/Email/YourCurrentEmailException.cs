using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Email
{
    public class YourCurrentEmailException() : Exception(ArabicErrorMessages.YourCurrentEmail)
    {
        public string ErrorCode { get; } = ErrorCodes.YourCurrentEmail;
        public string EnglishErrorMessage { get; } = EnglishErrorMessages.YourCurrentEmail;
    };
}
