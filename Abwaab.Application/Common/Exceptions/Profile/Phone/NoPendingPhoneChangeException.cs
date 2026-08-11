using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Phone
{
    public class NoPendingPhoneChangeException(): Exception(ErrorMessages.NoPendingPhoneChange)
    {
        public string ErrorCode { get; } = ErrorCodes.NoPendingPhoneChange;
        public string EnglishErrorMessage { get; } = ErrorMessagesEn.NoPendingPhoneChange;
    }
}
