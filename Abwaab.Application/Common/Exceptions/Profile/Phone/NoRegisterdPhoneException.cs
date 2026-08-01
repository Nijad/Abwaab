using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Profile.Phone
{
    public class NoRegisterdPhoneException() : Exception(ErrorMessages.NoRegisterdPhone)
    {
        public string ErrorCode { get; } = ErrorCodes.NoRegisterdPhone;
    };
}
