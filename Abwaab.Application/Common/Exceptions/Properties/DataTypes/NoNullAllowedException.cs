using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Properties.DataTypes
{
    public class NoNullAllowedException : NotAcceptable406Exception
    {
        public NoNullAllowedException(string title) : base(message: ErrorMessages.NoNullAllowed,
            title: title,
            errorCode: ErrorCodes.NoNullAllowed,
            returnToUser: true)
        {
        }
    }
}
