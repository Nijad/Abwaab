using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Role
{
    public class UserAlreadyInRoleException(string username, string roleName, string title) : BadRequest400Exception(
            message: "",
            title: title,
            errorCode: ErrorCodes.UserAlreadyInRole,
            returnToUser: true)
    {
        string msg = $"المستخدم '{username}' لديه بالفعل الدور '{roleName}'";
        public override string Message => msg;
    };
}
