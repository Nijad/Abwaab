using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Role
{
    public class FailedToAddUserToRoleException() : Exception(ArabicErrorMessages.FailedToAddUserToRole)
    {
        public string ErrorCode { get; } = ErrorCodes.FailedToAddUserToRole;
        public string EnglishErrorMessage { get; } = EnglishErrorMessages.FailedToAddUserToRole;
    };
}
