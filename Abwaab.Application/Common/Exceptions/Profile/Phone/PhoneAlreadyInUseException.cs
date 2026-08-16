using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Profile.Phone
{
    public class PhoneAlreadyInUseException(string title) : BadRequest400Exception(
            message: ErrorMessages.PhoneAlreadyInUse,
            title: title,
            errorCode: ErrorCodes.PhoneAlreadyInUse,
            returnToUser: true)
    {
    };
}
