using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.SMS
{
    public class FailedSendignSMSException(string title) : CusotomException(
            message: ErrorMessages.FailedSendingSms,
            title: title,
            errorCode: ErrorCodes.FailedSendingSms,
            returnToUser: true)
    {
    }
}
