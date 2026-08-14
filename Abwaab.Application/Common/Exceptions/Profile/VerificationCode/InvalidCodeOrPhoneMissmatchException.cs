using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.VerificationCode
{
    public class InvalidCodeOrPhoneMissmatchException(string title) : CusotomException(
            message: ErrorMessages.InvalidCodeOrPhoneMissmatch,
            title: title,
            errorCode: ErrorCodes.InvalidCodeOrPhoneMissmatch,
            returnToUser: true)
    {
    }
}
