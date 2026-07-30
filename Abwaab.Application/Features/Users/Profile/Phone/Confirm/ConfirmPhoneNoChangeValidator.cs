using FluentValidation;

namespace Abwaab.Application.Features.Users.Profile.Phone.Confirm
{
    public class ConfirmPhoneNoChangeValidator : AbstractValidator<ConfirmPhoneNoChangeCommand>
    {
        public ConfirmPhoneNoChangeValidator()
        {
            RuleFor(x => x.NewPhoneNo)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\+?[0-9]{8,15}$").WithMessage("Invalid phone number format.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Verification code is required.")
                .Length(6).WithMessage("Code must be 6 digits.")
                .Matches(@"^\d{6}$").WithMessage("Code must be numeric.");
        }
    }
}
