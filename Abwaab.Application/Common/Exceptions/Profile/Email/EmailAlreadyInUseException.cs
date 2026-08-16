using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Profile.Email
{
    public class EmailAlreadyInUseException(string title) : BadRequest400Exception(
            message: ErrorMessages.EmailAlreadyInUse,
            title: title,
            errorCode: ErrorCodes.EmailAlreadyInUse,
            returnToUser: true)
    {
    };
}
