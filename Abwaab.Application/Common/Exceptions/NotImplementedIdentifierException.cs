using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions
{
    public class NotImplementedIdentifierException(string identifierType, string title) : NotImplemented501Exception(
            message: "",
            title: title,
            errorCode: ErrorCodes.NotImplementdIdentifier,
            returnToUser: false)
    {
        string msg = $"المعرف من النوع '{identifierType}' ليس منجزاً بعد";
        public override string Message => msg;
    };
}
