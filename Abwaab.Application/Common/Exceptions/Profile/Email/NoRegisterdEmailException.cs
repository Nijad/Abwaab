using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Profile.Email
{
    public class NoRegisterdEmailException(string title) : PreconditionRequired428Exception(
            message: ErrorMessages.NoRegisterdEmail,
            title: title,
            errorCode: ErrorCodes.NoRegisterdEmail,
            returnToUser: true)
    {
    };
}
