using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Email
{
    public class EmailAlreadyInUseException(string title) : CusotomException(
            message: ErrorMessages.EmailAlreadyInUse,
            title: title,
            errorCode: ErrorCodes.EmailAlreadyInUse,
            returnToUser: true)
    {
    };
}
