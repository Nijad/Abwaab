using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Auth
{
    public class AccountLockedOutException() : Exception(ErrorMessages.AccountLocked)
    {
        public string ErrorCode { get; } = ErrorCodes.AccountLocked;
        public string EnglishErrorMessage { get; } = ErrorMessagesEn.AccountLocked;
    };
}
