using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Email
{
    public class FailedConfirmationEmailException() : Exception(ErrorMessages.FailedConfirmationEmail)
    {
        public string ErrorCode { get; } = ErrorCodes.FailedConfirmationEmail;
        public string EnglishErrorMessage { get; } = ErrorMessagesEn.FailedConfirmationEmail;
    }
}
