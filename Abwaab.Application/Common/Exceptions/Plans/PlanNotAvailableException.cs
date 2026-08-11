using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Plans
{
    public class PlanNotAvailableException() : Exception(ErrorMessages.PlanNotAvailable)
    {
        public string ErrorCode { get; } = ErrorCodes.PlanNotAvailable;
        public string EnglishErrorMessage { get; } = ErrorMessagesEn.PlanNotAvailable;
    }
}
