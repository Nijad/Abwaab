using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Validations.Common;
using FluentValidation;

namespace Abwaab.Application.Features.Users.Auth.Register
{
    public class RegisterUserValidation : AbstractValidator<RegisterUserDTO>
    {
        public RegisterUserValidation()
        {
            RuleFor(x => x.Identifier)
                .NotEmpty().WithMessage("المعرف مطلوب")
                .Must(CommonValidation.IsEmailOrPhoneNo).WithMessage("المعرف يجب أن يكون بريد الكتروني أو رقم موبايل (+9639XXXXXXXX)");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("كلمة المرور مطلوبة")
                .MinimumLength(8).WithMessage("كلمة المرور يجب أن تحتوي 8 محارف على الأقل")
                .Matches("[A-Z]").WithMessage("كلمة المرور يجب أن تحتوي على حرف انكليزي كبير واحد على الأقل")
                .Matches("[a-z]").WithMessage("كلمة المرور يجب أن تحتوي على حرف انكليزي صغير واحد على الأقل")
                .Matches("[0-9]").WithMessage("كلمة المرور يجب أن تحتوي على رقم واحد على الأقل");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage($"{GeneralConstants.FIRST_NAME} مطلوب")
                .MinimumLength(2).WithMessage($"{GeneralConstants.FIRST_NAME} يجب أن يحتوي حرفين على الأقل");
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage($"{GeneralConstants.LAST_NAME} مطلوبة")
                .MinimumLength(2).WithMessage($"{GeneralConstants.LAST_NAME} يجب أن تحتوي حرفين على الأقل");
            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("تأكيد كلمة المرور مطلوب")
                .Equal(x => x.Password).WithMessage("تأكيد كلمة المرور لا تتطابق مع كلمة المرور");
        }
    }
}
