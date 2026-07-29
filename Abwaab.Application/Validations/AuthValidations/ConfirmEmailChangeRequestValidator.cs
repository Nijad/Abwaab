using Abwaab.Application.DTOs.ApplicationUser.IdentifierManagement;
using FluentValidation;

namespace Abwaab.Application.Validations.AuthValidations
{
    public class ConfirmEmailChangeRequestValidator : AbstractValidator<ConfirmEmailChangeCommand>
    {
        public ConfirmEmailChangeRequestValidator()
        {
            RuleFor(x => x.NewEmail)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Verification code is required.")
                .Length(6).WithMessage("Code must be 6 digits.")
                .Matches(@"^\d{6}$").WithMessage("Code must be numeric.");
        }
    }
}
