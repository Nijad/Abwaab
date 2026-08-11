using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.VerificationCode
{
    public class InvalidCodeOrPhoneMissmatchException() : Exception(ErrorMessages.InvalidCodeOrPhoneMissmatch)
    {
        public string ErrorCode { get; } = ErrorCodes.InvalidCodeOrPhoneMissmatch;
        public string EnglishErrorMessage { get; } = ErrorMessagesEn.InvalidCodeOrPhoneMissmatch;
    }
}
