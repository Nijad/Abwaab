using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Properties.DataTypes
{
    public class DataTypeNotImplementedException : NotImplemented501Exception
    {
        public DataTypeNotImplementedException(string title) : base(message: ErrorMessages.DataTypeNotImplemented,
            title: title,
            errorCode: ErrorCodes.DataTypeNotImplemented,
            returnToUser: true)
        {
        }
    }
}
