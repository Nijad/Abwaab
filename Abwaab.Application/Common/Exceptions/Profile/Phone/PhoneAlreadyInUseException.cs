using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Phone
{
    public class PhoneAlreadyInUseException(string title) : CusotomException(
            message: ErrorMessages.PhoneAlreadyInUse,
            title: title,
            errorCode: ErrorCodes.PhoneAlreadyInUse,
            returnToUser: true)
    {
    };
}
