using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Payments
{
    public class NotValidPaymentCodeException(string title) : BadRequest400Exception(
            message: ErrorMessages.NotValidPaymentCode,
            title: title,
            errorCode: ErrorCodes.NotValidPaymentCode,
            returnToUser: true)
    {
    }
}
