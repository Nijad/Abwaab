using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Profile.VerificationCode
{
    public class InvalidCodeOrEmailMissmatchException(string title) : BadRequest400Exception(
            message: ErrorMessages.InvalidCodeOrEmailMismatch,
            title: title,
            errorCode: ErrorCodes.InvalidCodeOrEmailMismatch,
            returnToUser: true)
    {
    }
}
