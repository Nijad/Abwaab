using Abwaab.Application.Common.Constants;
namespace Abwaab.Application.Common.Exceptions.Profile.Plans
{
    public class UserAlreadyHasActivePlanException() : Exception(ArabicErrorMessages.UserAlreadyHasActivePlan)
    {
        public string ErrorCode { get; } = ErrorCodes.UserAlreadyHasActivePlan;
        public string EnglishErrorMessage { get; } = EnglishErrorMessages.UserAlreadyHasActivePlan;
    }
}
