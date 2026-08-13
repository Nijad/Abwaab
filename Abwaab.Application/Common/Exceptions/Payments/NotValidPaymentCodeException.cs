using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Payments
{
    public class NotValidPaymentCodeException() : Exception(ArabicErrorMessages.NotValidPaymentCode)
    {
        public string ErrorCode { get; } = ErrorCodes.NotValidPaymentCode;
        public string EnglishErrorMessage { get; } = EnglishErrorMessages.NotValidPaymentCode;
    }
}
