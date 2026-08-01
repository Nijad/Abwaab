using Microsoft.AspNetCore.Mvc;

namespace Abwaab.Server.Exceptions
{
    public class CustomProblemDetails : ProblemDetails
    {
        public string ErrorCode { get; set; } = string.Empty;
    }
}
