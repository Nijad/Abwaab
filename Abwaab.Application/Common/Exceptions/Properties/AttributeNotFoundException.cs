using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Properties
{
    public class AttributeNotFoundException : NotFound404Exception
    {
        public AttributeNotFoundException(string title) : base(message: ErrorMessages.AttributeNotFound,
            title: title,
            errorCode: ErrorCodes.AttributeNotFound,
            returnToUser: true)
        {
        }
    }
}
