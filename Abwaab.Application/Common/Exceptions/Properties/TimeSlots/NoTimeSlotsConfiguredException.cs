using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Properties.TimeSlots
{
    public class NoTimeSlotsConfiguredException(string title) :
        Forbidden403Exception(
            message: ErrorMessages.NoTimeSlotsConfigured,
            title: title,
            errorCode: ErrorCodes.NoTimeSlotsConfigured,
            returnToUser: true)
    {
    }
}
