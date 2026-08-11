using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.SMS
{
    public class FailedSendignSMSException() : Exception(ErrorMessages.FailedSendingSms)
    {
        public string ErrorCode { get; } = ErrorCodes.FailedSendingSms;
        public string EnglishErrorMessage { get; } = ErrorMessagesEn.FailedSendingSms;
    }
}
