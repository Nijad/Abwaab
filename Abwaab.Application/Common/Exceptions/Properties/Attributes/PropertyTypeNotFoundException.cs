using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Properties.Attributes
{
    public class PropertyTypeNotFoundException : NotFound404Exception
    {
        public PropertyTypeNotFoundException(string title) : base(message: ErrorMessages.PropertyTypeNotFound,
            title: title,
            errorCode: ErrorCodes.PropertyTypeNotFound,
            returnToUser: true)
        {
        }
    }
}
