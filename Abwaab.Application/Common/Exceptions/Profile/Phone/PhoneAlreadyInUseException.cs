using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Phone
{
    public class PhoneAlreadyInUseException() : Exception(ArabicErrorMessages.PhoneAlreadyInUse)
    {
        public string ErrorCode { get; } = ErrorCodes.PhoneAlreadyInUse;
        public string EnglishErrorMessage { get; } = EnglishErrorMessages.PhoneAlreadyInUse;
    };
}
