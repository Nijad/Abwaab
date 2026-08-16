using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Abwaab.Server.Exceptions
{
    public sealed class GlobalExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            ProblemDetails problem = exception switch
            {
                ValidationException ex => new ValidationProblemDetails(ex.Errors.GroupBy(g => g.PropertyName).ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()))
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "فشل التحقق",
                    Detail = "حدث خطأ واحد أو أكثر في التحقق من صحة المدخلات."
                },
                BadRequest400Exception ex => new CustomProblemDetails
                {
                    Status = ex.ReturnToUser ?
                        StatusCodes.Status400BadRequest :
                        StatusCodes.Status500InternalServerError,

                    Title = ex.Title,
                    Detail = ex.ReturnToUser ? ex.Message : ErrorMessages.SystemError,
                    ErrorCode = ex.ErrorCode
                },
                Unauthorized401Exception ex => new CustomProblemDetails
                {
                    Status = ex.ReturnToUser ?
                        StatusCodes.Status401Unauthorized :
                        StatusCodes.Status500InternalServerError,

                    Title = ex.Title,
                    Detail = ex.ReturnToUser ? ex.Message : ErrorMessages.SystemError,
                    ErrorCode = ex.ErrorCode
                },
                PaymentRequired402Exception ex => new CustomProblemDetails
                {
                    Status = ex.ReturnToUser ?
                        StatusCodes.Status402PaymentRequired :
                        StatusCodes.Status500InternalServerError,

                    Title = ex.Title,
                    Detail = ex.ReturnToUser ? ex.Message : ErrorMessages.SystemError,
                    ErrorCode = ex.ErrorCode
                },
                Forbidden403Exception ex => new CustomProblemDetails
                {
                    Status = ex.ReturnToUser ?
                        StatusCodes.Status403Forbidden :
                        StatusCodes.Status500InternalServerError,

                    Title = ex.Title,
                    Detail = ex.ReturnToUser ? ex.Message : ErrorMessages.SystemError,
                    ErrorCode = ex.ErrorCode
                },
                NotFound404Exception ex => new CustomProblemDetails
                {
                    Status = ex.ReturnToUser ?
                        StatusCodes.Status404NotFound :
                        StatusCodes.Status500InternalServerError,

                    Title = ex.Title,
                    Detail = ex.ReturnToUser ? ex.Message : ErrorMessages.SystemError,
                    ErrorCode = ex.ErrorCode
                },
                MethodNotAllowed405Exception ex => new CustomProblemDetails
                {
                    Status = ex.ReturnToUser ?
                        StatusCodes.Status405MethodNotAllowed :
                        StatusCodes.Status500InternalServerError,

                    Title = ex.Title,
                    Detail = ex.ReturnToUser ? ex.Message : ErrorMessages.SystemError,
                    ErrorCode = ex.ErrorCode
                },
                Precondition412Exception ex => new CustomProblemDetails
                {
                    Status = ex.ReturnToUser ?
                        StatusCodes.Status412PreconditionFailed :
                        StatusCodes.Status500InternalServerError,

                    Title = ex.Title,
                    Detail = ex.ReturnToUser ? ex.Message : ErrorMessages.SystemError,
                    ErrorCode = ex.ErrorCode
                },
                Locked423Exception ex => new CustomProblemDetails
                {
                    Status = ex.ReturnToUser ?
                        StatusCodes.Status423Locked :
                        StatusCodes.Status500InternalServerError,

                    Title = ex.Title,
                    Detail = ex.ReturnToUser ? ex.Message : ErrorMessages.SystemError,
                    ErrorCode = ex.ErrorCode
                },
                UpgradeRequired426Exception ex => new CustomProblemDetails
                {
                    Status = ex.ReturnToUser ?
                        StatusCodes.Status426UpgradeRequired :
                        StatusCodes.Status500InternalServerError,

                    Title = ex.Title,
                    Detail = ex.ReturnToUser ? ex.Message : ErrorMessages.SystemError,
                    ErrorCode = ex.ErrorCode
                },
                PreconditionRequired428Exception ex => new CustomProblemDetails
                {
                    Status = ex.ReturnToUser ?
                        StatusCodes.Status428PreconditionRequired :
                        StatusCodes.Status500InternalServerError,

                    Title = ex.Title,
                    Detail = ex.ReturnToUser ? ex.Message : ErrorMessages.SystemError,
                    ErrorCode = ex.ErrorCode
                },
                TooManyRequests429Exception ex => new CustomProblemDetails
                {
                    Status = ex.ReturnToUser ?
                        StatusCodes.Status429TooManyRequests :
                        StatusCodes.Status500InternalServerError,

                    Title = ex.Title,
                    Detail = ex.ReturnToUser ? ex.Message : ErrorMessages.SystemError,
                    ErrorCode = ex.ErrorCode
                },
                InternalServerError500Exception ex => new CustomProblemDetails
                { 
                    Status = ex.ReturnToUser ?
                        StatusCodes.Status500InternalServerError :
                        StatusCodes.Status500InternalServerError,

                    Title = ex.Title,
                    Detail = ex.ReturnToUser ? ex.Message : ErrorMessages.SystemError,
                    ErrorCode = ex.ErrorCode
                },
                NotImplemented501Exception ex => new CustomProblemDetails
                {
                    Status = ex.ReturnToUser ?
                        StatusCodes.Status501NotImplemented :
                        StatusCodes.Status500InternalServerError,

                    Title = ex.Title,
                    Detail = ex.ReturnToUser ? ex.Message : ErrorMessages.SystemError,
                    ErrorCode = ex.ErrorCode
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
