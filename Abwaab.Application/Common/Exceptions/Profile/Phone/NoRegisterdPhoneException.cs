using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Phone
{
    public class NoRegisterdPhoneException() : Exception(ArabicErrorMessages.NoRegisterdPhone)
    {
        public string ErrorCode { get; } = ErrorCodes.NoRegisterdPhone;
        public string EnglishErrorMessage { get;} = EnglishErrorMessages.NoRegisterdPhone;
    };
}
