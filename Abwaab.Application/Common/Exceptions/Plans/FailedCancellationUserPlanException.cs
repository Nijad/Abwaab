using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Plans
{
    public class FailedCancellationUserPlanException(string message, string title) : Precondition412Exception(
            message: message,
            title: title,
            errorCode: ErrorCodes.FailedCancelationUserPlan,
            returnToUser: true)
    {
    }
}
