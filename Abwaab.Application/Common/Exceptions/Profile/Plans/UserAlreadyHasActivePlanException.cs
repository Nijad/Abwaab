using Abwaab.Application.Common.Constants;
namespace Abwaab.Application.Common.Exceptions.Profile.Plans
{
    public class UserAlreadyHasActivePlanException() : Exception(ErrorMessages.UserAlreadyHasActivePlan)
    {
        public string ErrorCode { get; } = ErrorCodes.UserAlreadyHasActivePlan;
        public string EnglishErrorMessage { get; } = ErrorMessagesEn.UserAlreadyHasActivePlan;
    }
}
