using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Email
{
    public class EmailNotVerifiedException(string title) : CusotomException(
            message: ErrorMessages.EmailNotVerified,
            title: title,
            errorCode: ErrorCodes.EmailNotVerified,
            returnToUser: true)
    {
    };
}
