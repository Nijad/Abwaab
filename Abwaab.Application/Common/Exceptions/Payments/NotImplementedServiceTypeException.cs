using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Payments
{
    public class NotImplementedServiceTypeException(string message, string messageEn = "") : Exception(message)
    {
        public string ErrorCode { get; } = ErrorCodes.NotImplementedServiceType;
        public string EnglishErrorMessage { get; } = messageEn;
    }
}
