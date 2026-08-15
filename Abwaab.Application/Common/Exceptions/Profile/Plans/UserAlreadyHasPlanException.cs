using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;
namespace Abwaab.Application.Common.Exceptions.Profile.Plans
{
    public class UserAlreadyHasPlanException(string title) : BadRequest400Exception(
            message: ErrorMessages.UserAlreadyHasPlan,
            title: title,
            errorCode: ErrorCodes.UserAlreadyHasPlan,
            returnToUser: true)
    {
    }
}
