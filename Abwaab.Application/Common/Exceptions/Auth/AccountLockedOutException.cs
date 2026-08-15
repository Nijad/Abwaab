using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Auth
{
    public class AccountLockedOutException(string title) :
        Locked423Exception(
            message: ErrorMessages.AccountLocked,
            title: title,
            errorCode: ErrorCodes.AccountLocked,
            returnToUser: true)
    {
    };
}
