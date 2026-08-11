using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Auth
{
    public class UserAlreadyExistException() : Exception(ErrorMessages.UserAlreadyExist)
    {
        public string ErrorCode { get; } = ErrorCodes.UserAlreadyExist;
        public string EnglishErrorMessage { get; } = ErrorMessagesEn.UserAlreadyExist;
    };
}
