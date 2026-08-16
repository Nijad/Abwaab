using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Plans
{
    public class UserHasNoActivePlanException(string title) : PreconditionRequired428Exception(
            message: ErrorMessages.UserHasNoActivePlan,
            title: title,
            errorCode: ErrorCodes.UserHasNoActivePlan,
            returnToUser: true)
    {
    }
}
