using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Role
{
    public class FailedToAddUserToRoleException() : Exception(ErrorMessages.FailedToAddUserToRole)
    {
        public string ErrorCode { get; } = ErrorCodes.FailedToAddUserToRole;
    };
}
