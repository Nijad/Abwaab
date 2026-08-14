using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Email
{
    public class NoRegisterdEmailException(string title) : CusotomException(
            message: ErrorMessages.NoRegisterdEmail,
            title: title,
            errorCode: ErrorCodes.NoRegisterdEmail,
            returnToUser: true)
    {
    };
}
