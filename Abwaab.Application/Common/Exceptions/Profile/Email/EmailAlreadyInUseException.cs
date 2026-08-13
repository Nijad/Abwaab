using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Email
{
    public class EmailAlreadyInUseException() : Exception(ArabicErrorMessages.EmailAlreadyInUse)
    {
        public string ErrorCode { get; } = ErrorCodes.EmailAlreadyInUse;
        public string EnglishErrorMessage { get; } = EnglishErrorMessages.EmailAlreadyInUse;
    };
}
