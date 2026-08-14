using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Email
{
    public class FailedConfirmationEmailException(string title) : CusotomException(
            message: ErrorMessages.FailedConfirmationEmail,
            title: title,
            errorCode: ErrorCodes.FailedConfirmationEmail,
            returnToUser: true)
    {
    }
}
