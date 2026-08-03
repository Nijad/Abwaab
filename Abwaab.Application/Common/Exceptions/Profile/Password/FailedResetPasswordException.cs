using Abwaab.Application.Common.Constants;
namespace Abwaab.Application.Common.Exceptions.Profile.Password
{
    public class FailedResetPasswordException() :Exception(ErrorMessages.FailedResetPassword)
    {
        public string ErrorCode { get; } = ErrorCodes.FailedResetPassword;
    }
}
