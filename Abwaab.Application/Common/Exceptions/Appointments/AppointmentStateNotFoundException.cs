using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Appointments;

public class AppointmentStateNotFoundException(string title) :
    NotFound404Exception(
        message: ErrorMessages.AppointmentStateNotFound,
        title: title,
        errorCode: ErrorCodes.AppointmentStateNotFound,
        returnToUser: true)
{
}
