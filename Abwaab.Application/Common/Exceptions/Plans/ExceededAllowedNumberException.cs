using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Plans
{
    //todo: translate
    public class ExceededAllowedNumberException(string type, string planName) : Exception($"You have already added allowd number of {type} in plan '{planName}'")
    {
        public string ErrorCode { get; } = ErrorCodes.ExceededAllowedNumber;
        public string Title { get; } = $"Failed To Add {type}";
    }
}
