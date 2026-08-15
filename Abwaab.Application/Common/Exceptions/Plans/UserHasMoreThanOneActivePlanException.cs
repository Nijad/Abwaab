using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Plans
{
    public class UserHasMoreThanOneActivePlanException(string title) : Precondition412Exception(
            message: ErrorMessages.UserHasMoreThanOneActivePlan,
            title: title,
            errorCode: ErrorCodes.UserHasMoreThanOneActivePlan,
            returnToUser: true)
    {
    }
}
