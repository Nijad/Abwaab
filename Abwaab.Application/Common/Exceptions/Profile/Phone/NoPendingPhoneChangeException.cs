using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Phone
{
    public class NoPendingPhoneChangeException(string title) : CusotomException(
            message: ErrorMessages.NoPendingPhoneChange,
            title: title,
            errorCode: ErrorCodes.NoPendingPhoneChange,
            returnToUser: true)
    {
    }
}
