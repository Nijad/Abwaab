using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Abwaab.Server.Exceptions
{
    public sealed class AlternativeExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            ProblemDetails problem = exception switch
            {
                AccountLockedOutException ex => new CustomProblemDetails
                {
                    Detail = ex.Message,
                    Title = ex.Title,
                    ErrorCode = ErrorCodes.AccountLocked,
                    Status = ex.ReturnToUser ? 
                        StatusCodes.Status423Locked :
                        StatusCodes.Status500InternalServerError
                },
                ValidationException ex => new ValidationProblemDetails(ex.Errors.GroupBy(g => g.PropertyName).ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()))
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation Failed",
                    Detail = "One or more validation errors occurred."
                },
                _ => new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Server Error!",
                    Detail = "An unhandeled exception!"
                }
            };

            httpContext.Response.StatusCode = problem.Status!.Value;

            await problemDetailsService.WriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = problem
            });

            return true;
        }
    }
}
