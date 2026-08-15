using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Profile.Phone
{
    public class NoPendingPhoneChangeException(string title) : Precondition412Exception(
            message: ErrorMessages.NoPendingPhoneChange,
            title: title,
            errorCode: ErrorCodes.NoPendingPhoneChange,
            returnToUser: true)
    {
    }
}
