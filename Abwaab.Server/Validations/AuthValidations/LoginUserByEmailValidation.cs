using Abwaab.Application.DTOs.ApplicationUser;
using FluentValidation;

namespace Abwaab.Server.Validations.AuthValidations
{
    public class LoginUserByEmailValidation : AbstractValidator<LoginUserByEmailRequest>
    {
        public LoginUserByEmailValidation()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
