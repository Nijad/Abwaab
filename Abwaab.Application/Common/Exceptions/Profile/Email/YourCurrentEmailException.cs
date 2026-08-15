using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Profile.Email
{
    public class YourCurrentEmailException(string title) : BadRequest400Exception(
            message: ErrorMessages.YourCurrentEmail,
            title: title,
            errorCode: ErrorCodes.YourCurrentEmail,
            returnToUser: true)
    {
    };
}
