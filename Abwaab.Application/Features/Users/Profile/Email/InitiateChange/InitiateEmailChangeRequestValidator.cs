using FluentValidation;

namespace Abwaab.Application.Features.Users.Profile.Email.InitiateChange
{
    public class InitiateEmailChangeRequestValidator : AbstractValidator<InitiateEmailChangeCommand>
    {
        public InitiateEmailChangeRequestValidator()
        {
            RuleFor(x => x.NewEmail)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches("[A-Z]").WithMessage("Password must contain an uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain a lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain a number.");
        }
    }
}
