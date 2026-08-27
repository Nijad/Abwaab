using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Properties.TimeSlots
{
    public class TimeSlotNotFoundException : NotFound404Exception
    {
        public TimeSlotNotFoundException(string title) : base(message: ErrorMessages.TimeSlotNotFound,
            title: title,
            errorCode: ErrorCodes.TimeSlotNotFound,
            returnToUser: true)
        {
        }
    }
}
