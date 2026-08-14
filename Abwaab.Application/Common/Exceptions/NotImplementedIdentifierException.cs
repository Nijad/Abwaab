using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions
{
    public class NotImplementedIdentifierException(string identifierType, string title) : CusotomException(
            message: "",
            title: title,
            errorCode: ErrorCodes.NotImplementdIdentifier,
            returnToUser: false)
    {
        string msg = $"المعرف من النوع '{identifierType}' ليس منجزاً بعد";
        public override string Message => msg;
    };
}
