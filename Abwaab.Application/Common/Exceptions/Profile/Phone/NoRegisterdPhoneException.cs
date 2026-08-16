using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Profile.Phone
{
    public class NoRegisterdPhoneException(string title) : PreconditionRequired428Exception(
            message: ErrorMessages.NoRegisterdPhone,
            title: title,
            errorCode: ErrorCodes.NoRegisterdPhone,
            returnToUser: true)
    {
    };
}
