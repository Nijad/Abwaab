using Abwaab.Application.DTOs.ApplicationUser.IdentifierManagement;
using Abwaab.Application.Validations.Common;
using FluentValidation;

namespace Abwaab.Application.Validations.AuthValidations
{
    public class InitiatePhoneNoChangeValidator : AbstractValidator<InitiatePhoneNoChangeCommand>
    {
        public InitiatePhoneNoChangeValidator()
        {
            RuleFor(x => x.NewPhoneNo)
                .NotEmpty().WithMessage("Phone number is required.")
                .Must(CommonValidation.IsValidPhoneNumber).WithMessage("Phone number must be either valide email or valid phone number(+9639XXXXXXXX)");

            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches("[A-Z]").WithMessage("Password must contain an uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain a lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain a number.");
        }
    }
}
