using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Phone
{
    public class NoVerifiedPhoneException() : Exception(ArabicErrorMessages.NoVerifiedPhone)
    {
        public string ErrorCode { get; } = ErrorCodes.NoVerifiedPhone;
        public string EnglishErrorMessage { get; } = EnglishErrorMessages.NoVerifiedPhone;
    };
}
