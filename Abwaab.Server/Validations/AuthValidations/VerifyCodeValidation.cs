using Abwaab.Application.DTOs.ApplicationUser.VerificationCode;
using Abwaab.Infrastructure.Common;
using FluentValidation;

namespace Abwaab.Server.Validations.AuthValidations
{
    public class VerifyCodeValidation : AbstractValidator<VerifyCodeDTO>
    {
        public VerifyCodeValidation()
        {
            RuleFor(x => x.Identifier)
                .NotEmpty().WithMessage("Identifier is required.")
                .Must(CommonValidation.IsEmailOrPhoneNo).WithMessage("Identifier must be either valide email or valid phone number(+9639XXXXXXXX)");
            
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Verification code is required.")
                .Length(6).WithMessage("Verification code must be 6 digits long.");

            RuleFor(x=>x.Code)
                .Matches("^[0-9]{6}$").WithMessage("Verification code must be numeric."); 
        }
    }
}
