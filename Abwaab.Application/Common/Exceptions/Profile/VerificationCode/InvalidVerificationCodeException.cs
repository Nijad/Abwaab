using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.VerificationCode
{
    public class InvalidVerificationCodeException(string title) : CusotomException(
            message: ErrorMessages.InvalidVerificationCode,
            title: title,
            errorCode: ErrorCodes.InvalidVerificationCode,
            returnToUser: true)
    {
    }
}
