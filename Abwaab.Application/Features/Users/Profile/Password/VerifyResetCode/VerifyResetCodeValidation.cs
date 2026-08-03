using Abwaab.Application.Common.Validations.Common;
using FluentValidation;

namespace Abwaab.Application.Features.Users.Profile.Password.VerifyResetCode
{
    public class VerifyResetCodeValidation : AbstractValidator<VerifyResetCodeDTO>
    {
        public VerifyResetCodeValidation()
        {
            RuleFor(x => x.Identifier)
                .NotEmpty().WithMessage("Identifier is required.")
                .Must(CommonValidation.IsEmailOrPhoneNo).WithMessage("Identifier must be either valide email or valid phone number(+9639XXXXXXXX)");

            RuleFor(x => x.Code).NotEmpty().Length(6).Matches(@"^\d{6}$");
        }
    }
}
