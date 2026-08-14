using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Auth
{
    public class UserNotFoundException(string identifier, string title) :
        CusotomException(
            message: "",
            title: title,
            errorCode: ErrorCodes.UserNotFound,
            returnToUser: true)
    {
        string msg = $"المستخدم '{identifier} غير موجود'";
        public override string Message => msg;
    };
}
