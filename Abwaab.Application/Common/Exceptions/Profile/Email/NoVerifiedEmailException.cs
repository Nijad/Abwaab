using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Email
{
    public class NoVerifiedEmailException(string title) : CusotomException(
            message: ErrorMessages.NoVerifiedEmail,
            title: title,
            errorCode: ErrorCodes.NoVerifiedEmail,
            returnToUser: true)
    {
    };
}
