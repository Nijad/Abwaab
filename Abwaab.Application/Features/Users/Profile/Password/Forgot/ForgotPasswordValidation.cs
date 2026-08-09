using Abwaab.Application.Common.Validations.Common;
using FluentValidation;

namespace Abwaab.Application.Features.Users.Profile.Password.Forgot
{
    public class ForgotPasswordValidation : AbstractValidator<ForgotPasswordDTO>
    {
        public ForgotPasswordValidation()
        {
            RuleFor(x => x.Identifier)
                .NotEmpty().WithMessage("Identifier is required.")
                .Must(CommonValidation.IsEmailOrPhoneNo).WithMessage("Identifier must be either valide email or valid phone number(+9639XXXXXXXX)");
        }
    }
}
