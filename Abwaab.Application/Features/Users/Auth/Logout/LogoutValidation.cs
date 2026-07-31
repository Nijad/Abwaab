using FluentValidation;

namespace Abwaab.Application.Features.Users.Auth.Logout
{
    public class LogoutValidation : AbstractValidator<LogoutCommand>
    {
        public LogoutValidation()
        {
            // If RevokeAll is false, RefreshToken is required
            RuleFor(x => x.RefreshToken)
                .NotEmpty().When(x => !x.RevokeAll)
                .WithMessage("Refresh token is required when not revoking all.");
        }
    }
}
