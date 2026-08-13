using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.VerificationCode
{
    public class InvalidCodeOrEmailMissmatchException() : Exception(ArabicErrorMessages.InvalidCodeOrEmailMismatch)
    {
        public string ErrorCode { get; } = ErrorCodes.InvalidCodeOrEmailMismatch;
        public string EnglishErrorMessage { get; } = EnglishErrorMessages.InvalidCodeOrEmailMismatch;
    }
}
