using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Role
{
    public class UserAlreadyInRoleException(string username, string roleName, string title) : CusotomException(
            message: "",
            title: title,
            errorCode: ErrorCodes.UserAlreadyInRole,
            returnToUser: true)
    {
        string msg = $"المستخدم '{username}' لديه بالفعل الدور '{roleName}'";
        public override string Message => msg;
    };
}
