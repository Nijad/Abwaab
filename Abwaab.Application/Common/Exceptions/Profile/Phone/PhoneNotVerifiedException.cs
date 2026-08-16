using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Profile.Phone
{
    public class PhoneNotVerifiedException(string title) : PreconditionRequired428Exception(
            message: ErrorMessages.PhoneNotVerified,
            title: title,
            errorCode: ErrorCodes.PhoneNotVerified,
            returnToUser: true)
    {
    };
}
