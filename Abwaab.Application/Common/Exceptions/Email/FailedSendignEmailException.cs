using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Email
{
    public class FailedSendignEmailException() : Exception(ArabicErrorMessages.FailedSendingEmail)
    {
        public string ErrorCode { get; } = ErrorCodes.FailedSendingEmail;
        public string EnglishErrorMessage { get; } = EnglishErrorMessages.FailedSendingEmail;
    }
}
