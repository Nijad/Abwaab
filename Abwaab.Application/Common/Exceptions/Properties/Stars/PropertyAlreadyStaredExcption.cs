using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Properties.Stars
{
    public class PropertyAlreadyStaredExcption(string title) :
        BadRequest400Exception(
            message: ErrorMessages.PropertyAlreadyStared,
            title: title,
            errorCode: ErrorCodes.PropertyAlreadyStared,
            returnToUser: true)
    {
    };
}
