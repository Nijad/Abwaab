using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile
{
    public class FailedChangePasswordException() : Exception(ErrorMessages.FailedChangePassword)
    {
        public string ErrorCode { get; } = ErrorCodes.FailedChangePassword;
    };
}
