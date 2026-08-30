using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Appointments;

public class ChanginAppointmentStateNotAllowedException(string message, string title) :
    Precondition412Exception(
        message: "",
        title: title,
        errorCode: ErrorCodes.ChanginAppointmentStateNotAllowed,
        returnToUser: true)
{
    public override string Message => message;
}
