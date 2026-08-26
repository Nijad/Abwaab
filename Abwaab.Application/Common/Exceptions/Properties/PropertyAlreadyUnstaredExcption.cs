using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Properties
{
    public class PropertyAlreadyUnstaredExcption(string title) :
        BadRequest400Exception(
            message: ErrorMessages.PropertyAlreadyUnstared,
            title: title,
            errorCode: ErrorCodes.PropertyAlreadyUnstared,
            returnToUser: true)
    {
    };
}
