using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Payments
{
    public class NotImplementedServiceTypeException(string message, string title) : NotImplemented501Exception(
            message: message,
            title: title,
            errorCode: ErrorCodes.NotImplementedServiceType,
            returnToUser: false)
    {
    }
}
