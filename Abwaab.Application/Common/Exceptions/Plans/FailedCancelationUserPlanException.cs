using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Plans
{
    public class FailedCancelationUserPlanException(string message, string messageEn="") : Exception(message)
    {
        public string ErrorCode { get; } = ErrorCodes.FailedCancelationUserPlan;
        public string EnglishErrorMessage { get; } = messageEn;
    }
}
