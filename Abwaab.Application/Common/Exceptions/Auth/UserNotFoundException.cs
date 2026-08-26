using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Auth
{
    public class UserNotFoundException(string identifier, string title) :
        NotFound404Exception(
            message: "",
            title: title,
            errorCode: ErrorCodes.UserNotFound,
            returnToUser: true)
    {
        string msg = $"المستخدم '{identifier} غير موجود'";
        public override string Message => msg;
    }
}
