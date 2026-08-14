using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Auth
{
    public class AccountLockedOutException(string title) :
        CusotomException(
            message: ErrorMessages.AccountLocked,
            title: title,
            errorCode: ErrorCodes.AccountLocked,
            returnToUser: true)
    {
    };
}
