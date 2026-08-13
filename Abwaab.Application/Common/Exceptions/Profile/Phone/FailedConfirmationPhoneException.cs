using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Phone
{
    public class FailedConfirmationPhoneException() : Exception(ArabicErrorMessages.FailedConfirmationPhone)
    {
        public string ErrorCode { get; } = ErrorCodes.FailedConfirmationPhone;
        public string EnglishErrorMessage { get; } = EnglishErrorMessages.FailedConfirmationPhone;
    }
}
