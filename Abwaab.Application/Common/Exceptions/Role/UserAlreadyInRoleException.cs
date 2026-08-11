using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Role
{
    //todo: translate
    public class UserAlreadyInRoleException(string roleName) : Exception($"User already in role {roleName}")
    {
        public string ErrorCode { get; } = ErrorCodes.UserAlreadyInRole;
    };
}
