using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Phone
{
    public class YourCurrentPhoneException() : Exception(ErrorMessages.YourCurrentPhone)
    {
        public string ErrorCode { get; } = ErrorCodes.YourCurrentPhone;
        public string EnglishErrorMessage { get; } = ErrorMessagesEn.YourCurrentPhone;
    };
}
