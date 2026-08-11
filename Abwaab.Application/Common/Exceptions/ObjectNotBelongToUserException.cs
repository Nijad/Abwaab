using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions
{
    //todo: translate
    public class ObjectNotBelongToUserException(string objectType, string objectIdentifier, string identifierValue) : Exception($"{objectType} with {objectIdentifier} equal to {identifierValue} is not belong to you.")
    {
        public string ErrorCode { get; } = ErrorCodes.ObjectNotBelongToUser;
    }
}
