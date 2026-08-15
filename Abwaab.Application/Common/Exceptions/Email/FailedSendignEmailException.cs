using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Email
{
    public class FailedSendignEmailException(string title) : InternalServerError500Exception(
            message: ErrorMessages.FailedSendingEmail,
            title: title,
            errorCode: ErrorCodes.FailedSendingEmail,
            returnToUser: true)
    {
    }
}
