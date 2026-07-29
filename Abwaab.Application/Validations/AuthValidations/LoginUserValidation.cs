using Abwaab.Application.DTOs.ApplicationUser.LoginUser;
using Abwaab.Application.Validations.Common;
using FluentValidation;

namespace Abwaab.Application.Validations.AuthValidations
{
    public class LoginUserValidation : AbstractValidator<LoginUserDTO>
    {
        public LoginUserValidation()
        {
           RuleFor(x => x.Identifier)
                .NotEmpty().WithMessage("Identifier is required.")
                .Must(CommonValidation.IsEmailOrPhoneNo).WithMessage("Identifier must be either valide email or valid phone number(+9639XXXXXXXX)");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
