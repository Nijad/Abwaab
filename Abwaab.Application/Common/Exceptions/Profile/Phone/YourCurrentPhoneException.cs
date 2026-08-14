using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Phone
{
    public class YourCurrentPhoneException(string title) : CusotomException(
            message: ErrorMessages.YourCurrentPhone,
            title: title,
            errorCode: ErrorCodes.YourCurrentPhone,
            returnToUser: true)
    {
    };
}
