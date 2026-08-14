using Abwaab.Application.Common.Constants;
namespace Abwaab.Application.Common.Exceptions.Profile.Plans
{
    public class UserAlreadyHasActivePlanException(string title) : CusotomException(
            message: ErrorMessages.UserAlreadyHasActivePlan,
            title: title,
            errorCode: ErrorCodes.UserAlreadyHasActivePlan,
            returnToUser: true)
    {
    }
}
