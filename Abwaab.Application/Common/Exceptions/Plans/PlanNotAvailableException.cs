using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Plans
{
    public class PlanNotAvailableException(string title) : CusotomException(
            message: ErrorMessages.PlanNotAvailable,
            title: title,
            errorCode: ErrorCodes.PlanNotAvailable,
            returnToUser: false)
    {
    }
}
