using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Email
{
    public class EmailNotVerifiedException() : Exception(ErrorMessages.EmailNotVerified)
    {
        public string ErrorCode { get; } = ErrorCodes.EmailNotVerified;
        public string EnglishErrorMessage { get; } = ErrorMessagesEn.EmailNotVerified;
    };
}
