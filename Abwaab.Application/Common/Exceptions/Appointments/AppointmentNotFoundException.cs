using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Appointments;

public class AppointmentNotFoundException(string title) :
    NotFound404Exception(
        message: ErrorMessages.AppointmentNotFound,
        title: title,
        errorCode: ErrorCodes.AppointmentNotFound,
        returnToUser: true)
{
}
