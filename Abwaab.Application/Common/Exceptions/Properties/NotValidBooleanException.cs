using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Properties
{
    public class NotValidBooleanException : BadRequest400Exception
    {
        public NotValidBooleanException(string title) : base(message: ErrorMessages.NotValidBoolean,
            title: title,
            errorCode: ErrorCodes.NotValidBoolean,
            returnToUser: true)
        {
        }
    }
}
