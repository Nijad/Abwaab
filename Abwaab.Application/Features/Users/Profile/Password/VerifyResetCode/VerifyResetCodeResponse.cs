using Abwaab.Application.Features.Users.Profile.Password.Forgot;
using FluentValidation;

namespace Abwaab.Application.Features.Users.Profile.Password.VerifyResetCode
{
    public class VerifyResetCodeResponse : AbstractValidator<ForgotPasswordDTO>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
