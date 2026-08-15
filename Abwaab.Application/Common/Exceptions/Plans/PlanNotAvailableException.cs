using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Plans
{
    public class PlanNotAvailableException(string title) : Precondition412Exception(
            message: ErrorMessages.PlanNotAvailable,
            title: title,
            errorCode: ErrorCodes.PlanNotAvailable,
            returnToUser: true)
    {
    }
}
