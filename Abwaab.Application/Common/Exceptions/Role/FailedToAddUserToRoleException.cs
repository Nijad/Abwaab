using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Role
{
    public class FailedToAddUserToRoleException(string title) : BadRequest400Exception(
            message: ErrorMessages.FailedToAddUserToRole,
            title: title,
            errorCode: ErrorCodes.FailedToAddUserToRole,
            returnToUser: true)
    {
    };
}
