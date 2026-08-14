using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Email
{
    public class NoPendingEmailChangeException(string title): CusotomException(
            message: ErrorMessages.NoPendingEmailChange,
            title: title,
            errorCode: ErrorCodes.NoPendingEmailChange,
            returnToUser: true)
    {
    }
}
