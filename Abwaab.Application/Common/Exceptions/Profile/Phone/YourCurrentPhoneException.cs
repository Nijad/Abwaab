using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Profile.Phone
{
    public class YourCurrentPhoneException(string title) : BadRequest400Exception(
            message: ErrorMessages.YourCurrentPhone,
            title: title,
            errorCode: ErrorCodes.YourCurrentPhone,
            returnToUser: true)
    {
    };
}
