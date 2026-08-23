using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Properties
{
    public class AttributePossibleValueNotFoundException : NotFound404Exception
    {
        public AttributePossibleValueNotFoundException(string title) : base(message: ErrorMessages.AttributePossibleValueNotFound,
            title: title,
            errorCode: ErrorCodes.AttributePossibleValueNotFound,
            returnToUser: true)
        {
        }
    }
}
