using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Plans
{
    public class UserHasMoreThanOneActivePlanException(string title) : CusotomException(
            message: ErrorMessages.UserHasMoreThanOneActivePlan,
            title: title,
            errorCode: ErrorCodes.UserHasMoreThanOneActivePlan,
            returnToUser: true)
    {
    }
}
