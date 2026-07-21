using Abwaab.Application.DTOs.ApplicationUser;
using Abwaab.Infrastructure.Common;
using FluentValidation;

namespace Abwaab.Server.Validations.AuthValidations
{
    public class ResendCodeValidationCode : AbstractValidator<ResendCodeDTO>
    {
        public ResendCodeValidationCode()
        {
            RuleFor(x => x.Identifier)
                .NotEmpty().WithMessage("Identifier is required.")
                .Must(CommonValidation.IsEmailOrPhoneNo).WithMessage("Identifier must be either valide email or valid phone number(+9639XXXXXXXX)");
        }
    }
}
