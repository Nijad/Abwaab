using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.SMS
{
    public class FailedSendignSMSException(string title) : InternalServerError500Exception(
            message: ErrorMessages.FailedSendingSms,
            title: title,
            errorCode: ErrorCodes.FailedSendingSms,
            returnToUser: true)
    {
    }
}
