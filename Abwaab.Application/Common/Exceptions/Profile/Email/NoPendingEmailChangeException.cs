using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Email
{
    public class NoPendingEmailChangeException(): Exception(ErrorMessages.NoPendingEmailChange)
    {
        public string ErrorCode { get; } = ErrorCodes.NoPendingEmailChange;
    }
}
