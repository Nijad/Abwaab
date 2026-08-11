using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Email
{
    public class NoRegisterdEmailException() : Exception(ErrorMessages.NoRegisterdEmail)
    {
        public string ErrorCode { get; } = ErrorCodes.NoRegisterdEmail;
        public string EnglishErrorMessage { get; } = ErrorMessagesEn.NoRegisterdEmail;
    };
}
