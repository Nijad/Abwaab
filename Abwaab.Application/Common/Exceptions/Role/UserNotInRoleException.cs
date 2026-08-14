using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Role
{
    public class UserNotInRoleException(string username, string roleName, string title) : CusotomException(
            message: "",
            title: title,
            errorCode: ErrorCodes.UserNotInRole,
            returnToUser: true)
    {
        string msg = $"المستخدم '{username}' ليس لديه بالفعل الدور '{roleName}'";
        public override string Message => msg;
    };
}
