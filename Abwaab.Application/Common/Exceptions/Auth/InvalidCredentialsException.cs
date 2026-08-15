using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Auth
{
    public class InvalidCredentialsException(string title) :
        BadRequest400Exception(
            message: ErrorMessages.InvalidCredentials,
            title: title,
            errorCode: ErrorCodes.InvalidCredentials,
            returnToUser: true)
    {
    };
}
