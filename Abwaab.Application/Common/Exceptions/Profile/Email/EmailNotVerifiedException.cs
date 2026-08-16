using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Profile.Email
{
    public class EmailNotVerifiedException(string title) : PreconditionRequired428Exception(
            message: ErrorMessages.EmailNotVerified,
            title: title,
            errorCode: ErrorCodes.EmailNotVerified,
            returnToUser: true)
    {
    };
}
