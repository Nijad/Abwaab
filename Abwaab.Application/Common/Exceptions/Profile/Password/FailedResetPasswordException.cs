using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;
namespace Abwaab.Application.Common.Exceptions.Profile.Password
{
    public class FailedResetPasswordException(string title) : BadRequest400Exception(
            message: ErrorMessages.FailedResetPassword,
            title: title,
            errorCode: ErrorCodes.FailedResetPassword,
            returnToUser: true)
    {
    }
}
