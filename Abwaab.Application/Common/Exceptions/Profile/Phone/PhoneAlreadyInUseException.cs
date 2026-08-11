using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Phone
{
    public class PhoneAlreadyInUseException() : Exception(ErrorMessages.PhoneAlreadyInUse)
    {
        public string ErrorCode { get; } = ErrorCodes.PhoneAlreadyInUse;
        public string EnglishErrorMessage { get; } = ErrorMessagesEn.PhoneAlreadyInUse;
    };
}
