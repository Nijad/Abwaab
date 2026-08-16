using Abwaab.Application.Common.Validations.Common;
using FluentValidation;

namespace Abwaab.Application.Features.Users.Profile.Phone.InitiateChange
{
    public class InitiatePhoneNoChangeValidation : AbstractValidator<InitiatePhoneNoChangeCommand>
    {
        public InitiatePhoneNoChangeValidation()
        {
            RuleFor(x => x.NewPhoneNo)
                .NotEmpty().WithMessage("رقم الموبايل مطلوب.")
                .Must(CommonValidation.IsValidPhoneNumber).WithMessage("\"تنسيق رقم الهاتف خاطئ (+9639XXXXXXXX).\"");

            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("كلمة المرور مطلوبة")
                .MinimumLength(8).WithMessage("كلمة المرور يجب أن تحتوي 8 محارف على الأقل")
                .Matches("[A-Z]").WithMessage("كلمة المرور يجب أن تحتوي على حرف انكليزي كبير واحد على الأقل")
                .Matches("[a-z]").WithMessage("كلمة المرور يجب أن تحتوي على حرف انكليزي صغير واحد على الأقل")
                .Matches("[0-9]").WithMessage("كلمة المرور يجب أن تحتوي على رقم واحد على الأقل");
        }
    }
}
