using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Plans
{
    public class UserHasMoreThanOneActivePlanException() : Exception(ErrorMessages.UserHasMoreThanOneActivePlan)
    {
        public string ErrorCode { get; set; } = ErrorCodes.UserHasMoreThanOneActivePlan;
    }
}
