using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Profile.VerificationCode
{
    public class InvalidVerificationCodeException(string title) : BadRequest400Exception(
            message: ErrorMessages.InvalidVerificationCode,
            title: title,
            errorCode: ErrorCodes.InvalidVerificationCode,
            returnToUser: true)
    {
    }
}
