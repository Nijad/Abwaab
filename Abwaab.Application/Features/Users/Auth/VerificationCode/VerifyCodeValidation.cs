using Abwaab.Application.Common.Validations.Common;
using FluentValidation;

namespace Abwaab.Application.Features.Users.Auth.VerificationCode
{
    public class VerifyCodeValidation : AbstractValidator<VerifyCodeDTO>
    {
        public VerifyCodeValidation()
        {
            RuleFor(x => x.Identifier)
                .NotEmpty().WithMessage("المعرف مطلوب")
                .Must(CommonValidation.IsEmailOrPhoneNo).WithMessage("المعرف يجب أن يكون بريد الكتروني أو رقم موبايل (+9639XXXXXXXX)");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("رمز التحقق مطلوب")
                .Length(6).WithMessage("رمز التحقق يجب أن يكون مكوناً من 6 أرقام")
                .Matches("^[0-9]{6}$").WithMessage("رمز التحقق يجب أن يكون مكوناً من الأرقام فقط"); 
        }
    }
}
