using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Email
{
    public class NoRegisterdEmailException() : Exception(ArabicErrorMessages.NoRegisterdEmail)
    {
        public string ErrorCode { get; } = ErrorCodes.NoRegisterdEmail;
        public string EnglishErrorMessage { get; } = EnglishErrorMessages.NoRegisterdEmail;
    };
}
