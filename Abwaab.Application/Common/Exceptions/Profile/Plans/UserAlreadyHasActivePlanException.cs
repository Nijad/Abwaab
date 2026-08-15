using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;
namespace Abwaab.Application.Common.Exceptions.Profile.Plans
{
    public class UserAlreadyHasActivePlanException(string title) : BadRequest400Exception(
            message: ErrorMessages.UserAlreadyHasActivePlan,
            title: title,
            errorCode: ErrorCodes.UserAlreadyHasActivePlan,
            returnToUser: true)
    {
    }
}
