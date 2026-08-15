using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Profile.Email
{
    public class NoPendingEmailChangeException(string title): Precondition412Exception(
            message: ErrorMessages.NoPendingEmailChange,
            title: title,
            errorCode: ErrorCodes.NoPendingEmailChange,
            returnToUser: true)
    {
    }
}
