using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Auth
{
    public class UserAlreadyExistException(string title) :
        CusotomException(
            message: ErrorMessages.UserAlreadyExist,
            title: title,
            errorCode: ErrorCodes.UserAlreadyExist,
            returnToUser: true)
    {
    };
}
