using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Custom
{
    public class MethodNotAllowed405Exception(string message, string title, string errorCode, bool returnToUser) : Exception
    {
        public override string Message => message;
        public string Title { get; } = title;
        public string ErrorCode { get; } = 
            string.IsNullOrEmpty(errorCode) ?
            ErrorCodes.NotFound :
            errorCode;
        public bool ReturnToUser { get; } = returnToUser;
    }
}
