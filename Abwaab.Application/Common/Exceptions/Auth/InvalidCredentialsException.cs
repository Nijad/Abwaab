using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Auth
{
    public class InvalidCredentialsException(string title) :
        CusotomException(
            message: ErrorMessages.InvalidCredentials,
            title: title,
            errorCode: ErrorCodes.InvalidCredentials,
            returnToUser: true)
    {
    };
}
