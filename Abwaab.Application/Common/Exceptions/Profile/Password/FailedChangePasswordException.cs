using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Profile.Password
{
    public class FailedChangePasswordException(string title) : BadRequest400Exception(
            message: ErrorMessages.FailedChangePassword,
            title: title,
            errorCode: ErrorCodes.FailedChangePassword,
            returnToUser: true)
    {
    };
}
