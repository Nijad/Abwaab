using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Auth
{
    public class UpdateUserFailedException(string title) :
        BadRequest400Exception(
            message: ErrorMessages.UpdateUserFailed,
            title: title,
            errorCode: ErrorCodes.UpdateUserFailed,
            returnToUser: true)
    {
    }
}
