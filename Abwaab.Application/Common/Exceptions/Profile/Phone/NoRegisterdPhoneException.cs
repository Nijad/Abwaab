using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Phone
{
    public class NoRegisterdPhoneException(string title) : CusotomException(
            message: ErrorMessages.NoRegisterdPhone,
            title: title,
            errorCode: ErrorCodes.NoRegisterdPhone,
            returnToUser: true)
    {
    };
}
