using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Plans
{
    public class FailedCancellationUserPlanException(string message, string title) : CusotomException(
            message: message,
            title: title,
            errorCode: ErrorCodes.FailedCancelationUserPlan,
            returnToUser: true)
    {
    }
}
