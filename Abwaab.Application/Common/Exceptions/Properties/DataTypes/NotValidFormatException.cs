using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Properties.DataTypes
{
    public class NotValidFormatException : NotAcceptable406Exception
    {
        public NotValidFormatException(string title) : base(message: ErrorMessages.NotValidFormat,
            title: title,
            errorCode: ErrorCodes.NotValidFormat,
            returnToUser: true)
        {
        }
    }
}
