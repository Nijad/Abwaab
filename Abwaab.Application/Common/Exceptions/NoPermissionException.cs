using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions;

public class NoPermissionException(string message, string title) :
    Forbidden403Exception(
        message: "",
        title: title,
        errorCode: ErrorCodes.NoPermission,
        returnToUser: true)
{
    string msg = message;
    public override string Message => msg;
}
