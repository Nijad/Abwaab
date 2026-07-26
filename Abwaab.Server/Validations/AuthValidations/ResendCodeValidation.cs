using Abwaab.Application.DTOs.ApplicationUser;
using Abwaab.Server.Validations.Common;
using FluentValidation;

namespace Abwaab.Server.Validations.AuthValidations
{
    public class ResendCodeValidation : AbstractValidator<IdentifierDTO>
    {
        public ResendCodeValidation()
        {
            RuleFor(x => x.Identifier)
                .NotEmpty().WithMessage("Identifier is required.")
                .Must(CommonValidation.IsEmailOrPhoneNo).WithMessage("Identifier must be either valide email or valid phone number(+9639XXXXXXXX)");
        }
    }
}
