using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Email
{
    public class FailedSendignEmailException(string title) : CusotomException(
            message: ErrorMessages.FailedSendingEmail,
            title: title,
            errorCode: ErrorCodes.FailedSendingEmail,
            returnToUser: true)
    {
    }
}
