using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Appointments;

public class ConfirmationAppointmentNotAllowedException(string title) :
    Precondition412Exception(
        message: ErrorMessages.ConfirmationAppointmentNotAllowed,
        title: title,
        errorCode: ErrorCodes.ConfirmationAppointmentNotAllowed,
        returnToUser: true)
{
}
