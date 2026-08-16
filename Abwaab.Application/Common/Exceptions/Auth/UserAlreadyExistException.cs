using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Auth
{
    public class UserAlreadyExistException(string title) :
        BadRequest400Exception(
            message: ErrorMessages.UserAlreadyExist,
            title: title,
            errorCode: ErrorCodes.UserAlreadyExist,
            returnToUser: true)
    {
    };
}
