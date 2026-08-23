using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Properties
{
    public class NotValidNumberException : BadRequest400Exception
    {
        public NotValidNumberException(string title) : base(message: ErrorMessages.NotValidNumber,
            title: title,
            errorCode: ErrorCodes.NotValidNumber,
            returnToUser: true)
        {
        }
    }
}
