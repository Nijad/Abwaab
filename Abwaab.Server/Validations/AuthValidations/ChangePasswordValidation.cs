using Abwaab.Application.DTOs.ApplicationUser.ChangePassword;
using FluentValidation;

namespace Abwaab.Server.Validations.AuthValidations
{
    public class ChangePasswordValidation : AbstractValidator<ChangePasswordDTO>
    {
        public ChangePasswordValidation()
        {
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
