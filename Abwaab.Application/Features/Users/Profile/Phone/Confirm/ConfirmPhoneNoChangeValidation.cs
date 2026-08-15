using Abwaab.Application.Common.Validations.Common;
using FluentValidation;

namespace Abwaab.Application.Features.Users.Profile.Phone.Confirm
{
    public class ConfirmPhoneNoChangeValidation : AbstractValidator<ConfirmPhoneNoChangeCommand>
    {
        public ConfirmPhoneNoChangeValidation()
        {
            RuleFor(x => x.NewPhoneNo)
                .NotEmpty().WithMessage("رقم الموبايل مطلوب.")
                .Must(CommonValidation.IsValidPhoneNumber).WithMessage("تنسيق رقم الهاتف خاطئ (+9639XXXXXXXX).");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("رمز التحقق مطلوب")
                .Length(6).WithMessage("رمز التحقق يجب أن يكون مكوناً من 6 أرقام")
                .Matches("^[0-9]{6}$").WithMessage("رمز التحقق يجب أن يكون مكوناً من الأرقام فقط");
        }
    }
}
