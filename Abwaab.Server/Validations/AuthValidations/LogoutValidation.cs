using Abwaab.Application.DTOs.ApplicationUser.LogoutUser;
using FluentValidation;

namespace Abwaab.Server.Validations.AuthValidations
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
