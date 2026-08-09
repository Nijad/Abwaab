using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Role
{
    public class UserAlreadyInRoleException(string roleName) : Exception($"User already in role {roleName}")
    {
        public string ErrorCode { get; } = ErrorCodes.UserAlreadyInRole;
    };
}
