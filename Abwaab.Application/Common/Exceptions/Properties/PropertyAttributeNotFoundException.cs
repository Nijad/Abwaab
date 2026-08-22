using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Properties
{
    public class PropertyAttributeNotFoundException : NotFound404Exception
    {
        public PropertyAttributeNotFoundException(string title) : base(message: ErrorMessages.PropertyAttributeNotFound,
            title: title,
            errorCode: ErrorCodes.PropertyAttributeNotFound,
            returnToUser: true)
        {
        }
    }
}
