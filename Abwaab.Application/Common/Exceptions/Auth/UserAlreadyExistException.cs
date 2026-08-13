using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Auth
{
    public class UserAlreadyExistException() : Exception(ArabicErrorMessages.UserAlreadyExist)
    {
        public string ErrorCode { get; } = ErrorCodes.UserAlreadyExist;
        public string EnglishErrorMessage { get; } = EnglishErrorMessages.UserAlreadyExist;
    };
}
