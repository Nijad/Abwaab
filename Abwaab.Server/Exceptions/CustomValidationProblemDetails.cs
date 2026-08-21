using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Abwaab.Server.Exceptions
{
    public class CustomValidationProblemDetails: ValidationProblemDetails
    {
        public string ErrorCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public CustomValidationProblemDetails():base()
        {
            
        }
        public CustomValidationProblemDetails(ModelStateDictionary modelState) :base(modelState)
        {
            
        }
        public CustomValidationProblemDetails(Dictionary<string, string[]> errors):base(errors)
        {
            
        }
    }
}
