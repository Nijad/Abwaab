using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Payments
{
    public class NotValidPaymentCodeException() : Exception(ErrorMessages.NotValidPaymentCode)
    {
        public string ErrorCode { get; set; } = ErrorCodes.NotValidPaymentCode;
    }
}
