using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Profile.Email
{
    public class NoVerifiedEmailException(string title) : PreconditionRequired428Exception(
            message: ErrorMessages.NoVerifiedEmail,
            title: title,
            errorCode: ErrorCodes.NoVerifiedEmail,
            returnToUser: true)
    {
    };
}
