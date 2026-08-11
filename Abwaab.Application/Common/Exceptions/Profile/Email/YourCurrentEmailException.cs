using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Email
{
    public class YourCurrentEmailException() : Exception(ErrorMessages.YourCurrentEmail)
    {
        public string ErrorCode { get; } = ErrorCodes.YourCurrentEmail;
        public string EnglishErrorMessage { get; } = ErrorMessagesEn.YourCurrentEmail;
    };
}
