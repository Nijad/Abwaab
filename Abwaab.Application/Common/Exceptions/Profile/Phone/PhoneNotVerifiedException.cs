using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Phone
{
    public class PhoneNotVerifiedException() : Exception(ErrorMessages.PhoneNotVerified)
    {
        public string ErrorCode { get; } = ErrorCodes.PhoneNotVerified;
        public string EnglishErrorMessage { get; } = ErrorMessagesEn.PhoneNotVerified;
    };
}
