using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Role
{
    public class UserNotInRoleException(string roleName) : Exception($"User not in role {roleName}")
    {
        public string ErrorCode { get; } = ErrorCodes.UserNotInRole;
    };
}
