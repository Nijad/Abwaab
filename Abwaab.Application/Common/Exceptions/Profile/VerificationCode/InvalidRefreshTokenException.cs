using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.VerificationCode
{
    public class InvalidRefreshTokenException(string title) : CusotomException(
            message: ErrorMessages.InvalidRefreshToken,
            title: title,
            errorCode: ErrorCodes.InvalidRefreshToken,
            returnToUser: true)
    {
    }
}
