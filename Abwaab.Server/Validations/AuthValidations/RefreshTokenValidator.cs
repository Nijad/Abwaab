using Abwaab.Application.DTOs.ApplicationUser.RefreshToken;
using FluentValidation;

namespace Abwaab.Server.Validations.AuthValidations
{
    public class RefreshTokenValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required.");
        }
    }
}
