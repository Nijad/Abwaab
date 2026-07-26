using Abwaab.Application.DTOs.ApplicationUser.VerificationCode;
using Abwaab.Server.Validations.Common;
using FluentValidation;

namespace Abwaab.Server.Validations.AuthValidations
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
