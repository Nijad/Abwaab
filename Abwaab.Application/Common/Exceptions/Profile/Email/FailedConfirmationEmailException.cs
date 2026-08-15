using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Profile.Email
{
    public class FailedConfirmationEmailException(string title) : BadRequest400Exception(
            message: ErrorMessages.FailedConfirmationEmail,
            title: title,
            errorCode: ErrorCodes.FailedConfirmationEmail,
            returnToUser: true)
    {
    }
}
