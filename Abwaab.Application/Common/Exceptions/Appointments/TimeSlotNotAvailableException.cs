using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Appointments;

public class TimeSlotNotAvailableException(string title) :
    Precondition412Exception(
        message: ErrorMessages.TimeSlotNotAvailable,
        title: title,
        errorCode: ErrorCodes.TimeSlotNotAvailable,
        returnToUser: true)
{
}
