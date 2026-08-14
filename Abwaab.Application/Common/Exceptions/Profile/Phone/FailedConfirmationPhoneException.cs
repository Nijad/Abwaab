using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Phone
{
    public class FailedConfirmationPhoneException(string title) : CusotomException(
            message: ErrorMessages.FailedConfirmationPhone,
            title: title,
            errorCode: ErrorCodes.FailedConfirmationPhone,
            returnToUser: true)
    {
    }
}
