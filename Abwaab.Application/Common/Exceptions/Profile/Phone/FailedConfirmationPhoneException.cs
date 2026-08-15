using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Profile.Phone
{
    public class FailedConfirmationPhoneException(string title) : BadRequest400Exception(
            message: ErrorMessages.FailedConfirmationPhone,
            title: title,
            errorCode: ErrorCodes.FailedConfirmationPhone,
            returnToUser: true)
    {
    }
}
