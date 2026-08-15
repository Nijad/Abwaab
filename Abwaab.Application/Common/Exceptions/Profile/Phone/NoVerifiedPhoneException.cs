using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Profile.Phone
{
    public class NoVerifiedPhoneException(string title) : PreconditionRequired428Exception(
            message: ErrorMessages.NoVerifiedPhone,
            title: title,
            errorCode: ErrorCodes.NoVerifiedPhone,
            returnToUser: true)
    {
    };
}
