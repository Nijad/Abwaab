using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Role
{
    public class FailedToRemoveUserFromRoleException() : Exception(ErrorMessages.FailedToRemoveUserFromRole)
    {
        public string ErrorCode { get; } = ErrorCodes.FailedToRemoveUserFromRole;
    };
}
