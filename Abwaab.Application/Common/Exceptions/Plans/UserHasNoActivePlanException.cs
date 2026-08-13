using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Plans
{
    public class UserHasNoActivePlanException() : Exception(ArabicErrorMessages.UserHasNoActivePlan)
    {
        public string ErrorCode { get; } = ErrorCodes.UserHasNoActivePlan;
        public string EnglishErrorMessage { get; } = EnglishErrorMessages.UserHasNoActivePlan;
    }
}
