using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Payments
{
    public class NotImplementedServiceTypeException(string message, string title) : CusotomException(
            message: message,
            title: title,
            errorCode: ErrorCodes.NotImplementedServiceType,
            returnToUser: false)
    {
    }
}
