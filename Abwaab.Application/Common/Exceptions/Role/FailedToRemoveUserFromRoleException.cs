using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Role
{
    public class FailedToRemoveUserFromRoleException(string title) : CusotomException(
            message: ErrorMessages.FailedToRemoveUserFromRole,
            title: title,
            errorCode: ErrorCodes.FailedToRemoveUserFromRole,
            returnToUser: true)
    {
    };
}
