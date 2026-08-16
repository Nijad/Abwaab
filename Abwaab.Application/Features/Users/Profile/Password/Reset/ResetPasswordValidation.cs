using Abwaab.Application.Common.Validations.Common;
using FluentValidation;

namespace Abwaab.Application.Features.Users.Profile.Password.Reset
{
    public class ResetPasswordValidation
    {
        public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordDTO>
        {
            public ResetPasswordCommandValidator()
            {
                RuleFor(x => x.Identifier)
                .NotEmpty().WithMessage("المعرف مطلوب")
                .Must(CommonValidation.IsEmailOrPhoneNo).WithMessage("المعرف يجب أن يكون بريد الكتروني أو رقم موبايل (+9639XXXXXXXX)");

                RuleFor(x => x.Code)
                    .NotEmpty().WithMessage("رمز التحقق مطلوب")
                    .Length(6).WithMessage("رمز التحقق يجب أن يكون مكوناً من 6 أرقام")
                    .Matches("^[0-9]{6}$").WithMessage("رمز التحقق يجب أن يكون مكوناً من الأرقام فقط");

                RuleFor(x => x.NewPassword)
                     .NotEmpty().WithMessage("كلمة المرور مطلوبة")
                    .MinimumLength(8).WithMessage("كلمة المرور يجب أن تحتوي 8 محارف على الأقل")
                    .Matches("[A-Z]").WithMessage("كلمة المرور يجب أن تحتوي على حرف انكليزي كبير واحد على الأقل")
                    .Matches("[a-z]").WithMessage("كلمة المرور يجب أن تحتوي على حرف انكليزي صغير واحد على الأقل")
                    .Matches("[0-9]").WithMessage("كلمة المرور يجب أن تحتوي على رقم واحد على الأقل");

                RuleFor(x => x.ConfirmNewPassword)
                    .NotEmpty().WithMessage("تأكيد كلمة المرور يجب مطلوب")
                    .Equal(x => x.NewPassword).WithMessage("تأكيد كلمة المرور لا تتطابق مع كلمة المرور");
            }
        }
    }
}