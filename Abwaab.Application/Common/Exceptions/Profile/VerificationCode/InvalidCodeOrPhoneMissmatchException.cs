using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Profile.VerificationCode
{
    public class InvalidCodeOrPhoneMissmatchException(string title) : BadRequest400Exception(
            message: ErrorMessages.InvalidCodeOrPhoneMissmatch,
            title: title,
            errorCode: ErrorCodes.InvalidCodeOrPhoneMissmatch,
            returnToUser: true)
    {
    }
}
