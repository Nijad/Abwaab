using FluentValidation;

namespace Abwaab.Application.Features.Users.Profile.Email.Confirm
{
    public class ConfirmEmailChangeRequestValidator : AbstractValidator<ConfirmEmailChangeCommand>
    {
        public ConfirmEmailChangeRequestValidator()
        {
            RuleFor(x => x.NewEmail)
                .NotEmpty().WithMessage("البريد الالكتروني مطلوب")
                .EmailAddress().WithMessage("الإدخال لا يتطابق مع تنسيق البريد الالكتروني");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("رمز التحقق مطلوب")
                .Length(6).WithMessage("رمز التحقق يجب أن يكون مكوناً من 6 أرقام")
                .Matches("^[0-9]{6}$").WithMessage("رمز التحقق يجب أن يكون مكوناً من الأرقام فقط");
        }
    }
}
