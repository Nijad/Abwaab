using Abwaab.Application.Common.Constants;
namespace Abwaab.Application.Common.Exceptions.Profile.Password
{
    public class FailedResetPasswordException(string title) : CusotomException(
            message: ErrorMessages.FailedResetPassword,
            title: title,
            errorCode: ErrorCodes.FailedResetPassword,
            returnToUser: true)
    {
    }
}
