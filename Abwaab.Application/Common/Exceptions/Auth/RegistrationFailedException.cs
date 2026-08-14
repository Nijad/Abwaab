using Abwaab.Application.Common.Constants;
using Whipstaff.Core.Entities;

namespace Abwaab.Application.Common.Exceptions.Auth
{
    public class RegistrationFailedException(string title) :
        CusotomException(
            message: ErrorMessages.RegistrationFailed,
            title: title,
            errorCode: ErrorCodes.RegistrationFailed,
            returnToUser: true)
    {
    }
}
