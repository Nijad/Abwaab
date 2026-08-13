using Abwaab.Application.Common.Constants;
namespace Abwaab.Application.Common.Exceptions.Profile.Password
{
    public class FailedResetPasswordException() :Exception(ArabicErrorMessages.FailedResetPassword)
    {
        public string ErrorCode { get; } = ErrorCodes.FailedResetPassword;
        public string EnglishErrorMessage { get; } = EnglishErrorMessages.FailedResetPassword;
    }
}
