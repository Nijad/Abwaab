using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.VerificationCode
{
    public class InvalidCodeOrEmailMissmatchException(string title) : CusotomException(
            message: ErrorMessages.InvalidCodeOrEmailMismatch,
            title: title,
            errorCode: ErrorCodes.InvalidCodeOrEmailMismatch,
            returnToUser: true)
    {
    }
}
