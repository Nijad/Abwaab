using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Email
{
    public class EmailAlreadyInUseException() : Exception(ErrorMessages.EmailAlreadyInUse)
    {
        public string ErrorCode { get; } = ErrorCodes.EmailAlreadyInUse;
    };
}
