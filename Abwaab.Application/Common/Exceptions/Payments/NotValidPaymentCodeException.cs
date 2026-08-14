using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Payments
{
    public class NotValidPaymentCodeException(string title) : CusotomException(
            message: ErrorMessages.NotValidPaymentCode,
            title: title,
            errorCode: ErrorCodes.NotValidPaymentCode,
            returnToUser: true)
    {
    }
}
