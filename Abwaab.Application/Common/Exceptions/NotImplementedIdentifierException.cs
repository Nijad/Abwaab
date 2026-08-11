using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions
{
    //todo: translate
    public class NotImplementedIdentifierException(string identifierType) : Exception($"Identifier type of {identifierType} does not implemented yet.")
    {
        public string ErrorCode { get; } = ErrorCodes.NotImplementdIdentifier;
    };
}
