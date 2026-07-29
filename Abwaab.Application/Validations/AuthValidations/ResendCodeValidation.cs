using Abwaab.Application.DTOs.ApplicationUser.VerificationCode;
using Abwaab.Application.Validations.Common;
using FluentValidation;

namespace Abwaab.Application.Validations.AuthValidations
{
    public class ResendCodeValidation : AbstractValidator<ResendCodeDTO>
    {
        public ResendCodeValidation()
        {
            RuleFor(x => x.Identifier)
                .NotEmpty().WithMessage("Identifier is required.")
                .Must(CommonValidation.IsEmailOrPhoneNo).WithMessage("Identifier must be either valide email or valid phone number(+9639XXXXXXXX)");
        }
    }
}
