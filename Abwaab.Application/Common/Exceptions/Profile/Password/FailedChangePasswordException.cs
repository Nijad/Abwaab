using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Password
{
    public class FailedChangePasswordException() : Exception(ErrorMessages.FailedChangePassword)
    {
        public string ErrorCode { get; } = ErrorCodes.FailedChangePassword;
        public string EnglishErrorMessage { get; } = ErrorMessagesEn.FailedChangePassword;
    };
}
