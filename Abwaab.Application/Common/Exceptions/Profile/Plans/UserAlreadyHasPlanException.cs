using Abwaab.Application.Common.Constants;
namespace Abwaab.Application.Common.Exceptions.Profile.Plans
{
    public class UserAlreadyHasPlanException() : Exception(ErrorMessages.UserAlreadyHasPlan)
    {
        public string ErrorCode { get; set; } = ErrorCodes.UserAlreadyHasPlan;
    }
}
