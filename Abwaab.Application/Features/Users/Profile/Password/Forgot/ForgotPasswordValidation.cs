using Abwaab.Application.Common.Validations.Common;
using FluentValidation;

namespace Abwaab.Application.Features.Users.Profile.Password.Forgot
{
    public class ForgotPasswordValidation : AbstractValidator<ForgotPasswordDTO>
    {
        public ForgotPasswordValidation()
        {
            RuleFor(x => x.Identifier)
                .NotEmpty().WithMessage("المعرف مطلوب")
                .Must(CommonValidation.IsEmailOrPhoneNo).WithMessage("المعرف يجب أن يكون بريد الكتروني أو رقم موبايل (+9639XXXXXXXX)");
        }
    }
}
