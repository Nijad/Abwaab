using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Common.Exceptions.Email;
using Abwaab.Application.Common.Exceptions.Plans;
using Abwaab.Application.Common.Exceptions.Profile;
using Abwaab.Application.Common.Exceptions.Profile.Email;
using Abwaab.Application.Common.Exceptions.Profile.NotificationWay;
using Abwaab.Application.Common.Exceptions.Profile.Password;
using Abwaab.Application.Common.Exceptions.Profile.Phone;
using Abwaab.Application.Common.Exceptions.Profile.Plans;
using Abwaab.Application.Common.Exceptions.Profile.VerificationCode;
using Abwaab.Application.Common.Exceptions.Role;
using Abwaab.Application.Common.Exceptions.SMS;
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
                UserAlreadyHasActivePlanException ex => new CustomProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Failed To Active Default Plan",
                    Detail = ex.Message,
                    ErrorCode = ex.ErrorCode
                },
                PlanNotAvailableException ex => new CustomProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Failed Upgreading Plan",
                    Detail = ex.Message,
                    ErrorCode = ex.ErrorCode
                },
                UserAlreadyHasPlanException ex => new CustomProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Failed Upgreading Plan",
                    Detail = ex.Message,
                    ErrorCode = ex.ErrorCode
                },
                FailedResetPasswordException ex => new CustomProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Failed Reset Password",
                    Detail = ex.Message,
                    ErrorCode = ex.ErrorCode
                },
                EmailNotVerifiedException ex => new CustomProblemDetails
                {
                    Status = StatusCodes.Status428PreconditionRequired,
                    Title = "Login Failed",
                    Detail = ex.Message,
                    ErrorCode = ex.ErrorCode
                },
                UserNotInRoleException ex => new CustomProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "User not in role",
                    Detail = ex.Message,
                    ErrorCode = ex.ErrorCode
                },
                UserAlreadyInRoleException ex => new CustomProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "User already in role",
                    Detail = ex.Message,
                    ErrorCode = ex.ErrorCode
                },
                FailedToRemoveUserFromRoleException ex => new CustomProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Failed to remove user from role",
                    Detail = ex.Message,
                    ErrorCode = ex.ErrorCode
                },
                FailedToAddUserToRoleException ex => new CustomProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Failed to add user to role",
                    Detail = ex.Message,
                    ErrorCode = ex.ErrorCode
                },
                FailedChangePasswordException ex => new CustomProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Failed changing password",
                    Detail = ex.Message,
                    ErrorCode = ex.ErrorCode
                },
                AlreadySubscribeNotificationWayException ex => new CustomProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Subscription Notification Way",
                    Detail = ex.Message,
                    ErrorCode = ex.ErrorCode
                },
                YourCurrentEmailException ex => new CustomProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Failed Update Email",
                    Detail = ex.Message,
                    ErrorCode = ex.ErrorCode
                },
                YourCurrentPhoneException ex => new CustomProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Failed Update Phone",
                    Detail = ex.Message,
                    ErrorCode = ex.ErrorCode
                },
                PhoneAlreadyInUseException ex => new CustomProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Failed Confirmation Phone",
                    Detail = ex.Message,
                    ErrorCode = ex.ErrorCode
                },
                EmailAlreadyInUseException ex => new CustomProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Failed Confirmation Email",
                    Detail = ex.Message,
                    ErrorCode = ex.ErrorCode
                },
                InvalidCodeOrPhoneMissmatchException ex => new CustomProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Failed Confirmation Phone",
                    Detail = ex.Message,
                    ErrorCode = ex.ErrorCode
                },
                InvalidCodeOrEmailMissmatchException ex => new CustomProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Failed Confirmation Email",
                    Detail = ex.Message,
                    ErrorCode = ex.ErrorCode
                },
                NoPendingPhoneChangeException ex => new CustomProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "No Pending Phone Change",
                    Detail = ex.Message,
                    ErrorCode = ex.ErrorCode
                },
                NoPendingEmailChangeException ex => new CustomProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "No Pending Email Change",
                    Detail = ex.Message,
                    ErrorCode = ex.ErrorCode
                },
                InvalidRefreshTokenException ex => new CustomProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Invalid Refresh Token",
                    Detail = ex.Message,
                    ErrorCode = ex.ErrorCode
                },
                NotImplementedIdentifierException ex => new CustomProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Not Implemented Identifier type",
                    Detail = ex.Message,
                    ErrorCode = ex.ErrorCode
                },
                FailedConfirmationPhoneException ex => new CustomProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Failed confirmation phnoe",
                    Detail = ex.Message,
                    ErrorCode = ex.ErrorCode
                },
                FailedConfirmationEmailException ex => new CustomProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Failed confirmation email",
                    Detail = ex.Message,
                    ErrorCode = ex.ErrorCode
                },
                InvalidVerificationCodeException ex => new CustomProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid Verification Code",
                    Detail = ex.Message,
                    ErrorCode = ex.ErrorCode
                },
                FailedSendignSMSException ex => new CustomProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Failed sending SMS",
                    Detail = ex.Message,
                    ErrorCode = ex.ErrorCode
                },
                FailedSendignEmailException ex => new CustomProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Failed sending email",
                    Detail = ex.Message,
                    ErrorCode = ex.ErrorCode
                },
                UserAlreadyExistException ex => new CustomProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "User already exist",
                    Detail = ex.Message,
                    ErrorCode = ex.ErrorCode
                },
                AccountLockedOutException ex=>new CustomProblemDetails
                {
                    Status = StatusCodes.Status403Forbidden,
                    Title = "Account Locked Out",
                    Detail = exception.Message,
                    ErrorCode = ex.ErrorCode
                },
                NotFoundException ex => new CustomProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = ex.Title,
                    Detail = ex.Message,
                    ErrorCode = ex.ErrorCode
                },
                InvalidCredentialsException ex => new CustomProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid Cedentials",
                    Detail = ex.Message,
                    ErrorCode = ex.ErrorCode
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
