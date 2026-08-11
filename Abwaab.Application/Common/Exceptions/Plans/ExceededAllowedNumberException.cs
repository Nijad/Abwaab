using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Plans
{
    public class ExceededAllowedNumberException(string type, string planName) : Exception($"You have already added allowd number of {type} in plan '{planName}'")
    {
        public string ErrorCode { get; set; } = ErrorCodes.ExceededAllowedNumber;
        public string Title { get; set; } = $"Failed To Add {type}";
    }
}
