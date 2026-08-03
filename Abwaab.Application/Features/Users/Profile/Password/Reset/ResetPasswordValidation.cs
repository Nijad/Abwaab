using Abwaab.Application.Common.Validations.Common;
using FluentValidation;

namespace Abwaab.Application.Features.Users.Profile.Password.Reset
{
    public class ResetPasswordValidation
    {
        public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordDTO>
        {
            public ResetPasswordCommandValidator()
            {
                RuleFor(x => x.Identifier)
                .NotEmpty().WithMessage("Identifier is required.")
                .Must(CommonValidation.IsEmailOrPhoneNo).WithMessage("Identifier must be either valide email or valid phone number(+9639XXXXXXXX)");

                RuleFor(x => x.Code).NotEmpty().Length(6).Matches(@"^\d{6}$");

                RuleFor(x => x.NewPassword)
                    .NotEmpty().WithMessage("Password is required.")
                    .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                    .Matches("[A-Z]").WithMessage("Password must contain an uppercase letter.")
                    .Matches("[a-z]").WithMessage("Password must contain a lowercase letter.")
                    .Matches("[0-9]").WithMessage("Password must contain a number.");

                RuleFor(x => x.ConfirmNewPassword)
                    .NotEmpty().WithMessage("Confirm Password is required.")
                    .Equal(x => x.NewPassword).WithMessage("Passwords do not match.");
            }
        }
    }
}