using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Appointments;

public class NotPublishedPropertyException(string title) :
    Precondition412Exception(
        message: ErrorMessages.NotPublishedProperty,
        title: title,
        errorCode: ErrorCodes.NotPublishedProperty,
        returnToUser: true)
{
}
