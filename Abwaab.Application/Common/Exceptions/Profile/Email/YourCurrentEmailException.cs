using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Email
{
    public class YourCurrentEmailException(string title) : CusotomException(
            message: ErrorMessages.YourCurrentEmail,
            title: title,
            errorCode: ErrorCodes.YourCurrentEmail,
            returnToUser: true)
    {
    };
}
