using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Properties
{
    public class PropertyNotFoundException : NotFound404Exception
    {
        public PropertyNotFoundException(string title) : base(message: ErrorMessages.PropertyNotFound,
            title: title,
            errorCode: ErrorCodes.PropertyNotFound,
            returnToUser: true)
        {
        }
    }
}
