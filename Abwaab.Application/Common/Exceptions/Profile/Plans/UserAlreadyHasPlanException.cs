using Abwaab.Application.Common.Constants;
namespace Abwaab.Application.Common.Exceptions.Profile.Plans
{
    public class UserAlreadyHasPlanException(string title) : CusotomException(
            message: ErrorMessages.UserAlreadyHasPlan,
            title: title,
            errorCode: ErrorCodes.UserAlreadyHasPlan,
            returnToUser: true)
    {
    }
}
