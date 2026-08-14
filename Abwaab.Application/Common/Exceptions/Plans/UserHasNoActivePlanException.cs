using Abwaab.Application.Common.Constants;
using Whipstaff.Core.Entities;

namespace Abwaab.Application.Common.Exceptions.Plans
{
    public class UserHasNoActivePlanException(string title) : CusotomException(
            message: ErrorMessages.UserHasNoActivePlan,
            title: title,
            errorCode: ErrorCodes.UserHasNoActivePlan,
            returnToUser: true)
    {
    }
}
