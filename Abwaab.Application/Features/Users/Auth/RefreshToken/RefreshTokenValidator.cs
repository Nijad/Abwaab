using FluentValidation;

namespace Abwaab.Application.Features.Users.Auth.RefreshToken
{
    public class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("رمز التحديث مطلوب");
        }
    }
}
