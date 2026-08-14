using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Phone
{
    public class NoVerifiedPhoneException(string title) : CusotomException(
            message: ErrorMessages.NoVerifiedPhone,
            title: title,
            errorCode: ErrorCodes.NoVerifiedPhone,
            returnToUser: true)
    {
    };
}
