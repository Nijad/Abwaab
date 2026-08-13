using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.SMS
{
    public class FailedSendignSMSException() : Exception(ArabicErrorMessages.FailedSendingSms)
    {
        public string ErrorCode { get; } = ErrorCodes.FailedSendingSms;
        public string EnglishErrorMessage { get; } = EnglishErrorMessages.FailedSendingSms;
    }
}
