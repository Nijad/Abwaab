using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Plans
{
    public class FailedCancelationUserPlanException(string message) : Exception(message)
    {
        public string ErrorCode { get; set; } = ErrorCodes.FailedCancelationUserPlan;
    }
}
