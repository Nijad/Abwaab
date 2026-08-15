using Abwaab.Application.Common.Validations.Common;
using FluentValidation;

namespace Abwaab.Application.Features.Users.Auth.Login
{
    public class LoginUserValidation : AbstractValidator<LoginUserDTO>
    {
        public LoginUserValidation()
        {
           RuleFor(x => x.Identifier)
                .NotEmpty().WithMessage("المعرف مطلوب")
                .Must(CommonValidation.IsEmailOrPhoneNo).WithMessage("المعرف يجب أن يكون بريد الكتروني أو رقم موبايل (+9639XXXXXXXX)");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("كلمة المرور مطلوبة");
        }
    }
}
