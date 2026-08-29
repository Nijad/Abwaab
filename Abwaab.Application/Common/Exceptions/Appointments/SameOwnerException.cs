using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Appointments;

public class SameOwnerException(string title) :
    NotFound404Exception(
        message: ErrorMessages.SameOwner,
        title: title,
        errorCode: ErrorCodes.SameOwner,
        returnToUser: true)
{
}
