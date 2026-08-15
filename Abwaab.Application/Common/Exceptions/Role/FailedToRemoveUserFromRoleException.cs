using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Role
{
    public class FailedToRemoveUserFromRoleException(string title) : BadRequest400Exception(
            message: ErrorMessages.FailedToRemoveUserFromRole,
            title: title,
            errorCode: ErrorCodes.FailedToRemoveUserFromRole,
            returnToUser: true)
    {
    };
}
