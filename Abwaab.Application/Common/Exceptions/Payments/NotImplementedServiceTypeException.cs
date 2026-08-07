using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Payments
{
    public class NotImplementedServiceTypeException(string message) : Exception(message)
    {
        public string ErrorCode { get; set; } = ErrorCodes.NotImplementedServiceType;
    }
}
