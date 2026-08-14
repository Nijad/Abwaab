using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Role
{
    public class FailedToAddUserToRoleException(string title) : CusotomException(
            message: ErrorMessages.FailedToAddUserToRole,
            title: title,
            errorCode: ErrorCodes.FailedToAddUserToRole,
            returnToUser: true)
    {
    };
}
