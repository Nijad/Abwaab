using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Phone
{
    public class NoVerifiedPhoneException() : Exception(ErrorMessages.NoVerifiedPhone)
    {
        public string ErrorCode { get; } = ErrorCodes.NoVerifiedPhone;
        public string EnglishErrorMessage { get; } = ErrorMessagesEn.NoVerifiedPhone;
    };
}
