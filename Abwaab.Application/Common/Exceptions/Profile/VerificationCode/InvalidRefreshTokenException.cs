using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Profile.VerificationCode
{
    public class InvalidRefreshTokenException(string title) : BadRequest400Exception(
            message: ErrorMessages.InvalidRefreshToken,
            title: title,
            errorCode: ErrorCodes.InvalidRefreshToken,
            returnToUser: true)
    {
    }
}
