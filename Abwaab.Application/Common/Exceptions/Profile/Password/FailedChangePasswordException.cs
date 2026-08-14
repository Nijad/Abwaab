using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Password
{
    public class FailedChangePasswordException(string title) : CusotomException(
            message: ErrorMessages.FailedChangePassword,
            title: title,
            errorCode: ErrorCodes.FailedChangePassword,
            returnToUser: true)
    {
    };
}
