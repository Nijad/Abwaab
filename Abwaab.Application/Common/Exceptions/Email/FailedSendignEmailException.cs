using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Email
{
    public class FailedSendignEmailException() : Exception(ErrorMessages.FailedSendingEmail)
    {
        public string ErrorCode { get; } = ErrorCodes.FailedSendingEmail;
    }
}
