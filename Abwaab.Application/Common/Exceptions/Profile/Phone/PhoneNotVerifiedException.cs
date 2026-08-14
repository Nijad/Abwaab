using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Phone
{
    public class PhoneNotVerifiedException(string title) : CusotomException(
            message: ErrorMessages.PhoneNotVerified,
            title: title,
            errorCode: ErrorCodes.PhoneNotVerified,
            returnToUser: true)
    {
    };
}
