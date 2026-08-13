using Abwaab.Application.Common.Constants;
namespace Abwaab.Application.Common.Exceptions.Profile.Plans
{
    public class UserAlreadyHasPlanException() : Exception(ArabicErrorMessages.UserAlreadyHasPlan)
    {
        public string ErrorCode { get; } = ErrorCodes.UserAlreadyHasPlan;
        public string EnglishErrorMessage { get; } = EnglishErrorMessages.UserAlreadyHasPlan;
    }
}
