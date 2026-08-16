using FluentValidation;

namespace Abwaab.Application.Features.Users.Profile.Password.Change
{
    public class ChangePasswordValidation : AbstractValidator<ChangePasswordDTO>
    {
        public ChangePasswordValidation()
        {
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
