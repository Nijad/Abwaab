using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Interfaces
{
    public interface IDefinedException
    {
        public string ErrorCode { get; }
        public bool ReturnToUser { get; }
        public int Status { get; }
        //public string Message { get; init; }
        //public string Title { get; init; }
    }
}
