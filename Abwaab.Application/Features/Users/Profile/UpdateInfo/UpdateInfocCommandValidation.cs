using Abwaab.Application.Common.Constants;
using FluentValidation;

namespace Abwaab.Application.Features.Users.Profile.UpdateInfo
{
    public class UpdateInfocCommandValidation : AbstractValidator<UpdateInfoCommand>
    {
        public UpdateInfocCommandValidation()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage($"{GeneralConstants.FIRST_NAME} مطلوب")
                .MinimumLength(2).WithMessage($"{GeneralConstants.FIRST_NAME} يجب أن يحتوي حرفين على الأقل");
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage($"{GeneralConstants.LAST_NAME} مطلوبة")
                .MinimumLength(2).WithMessage($"{GeneralConstants.LAST_NAME} يجب أن تحتوي حرفين على الأقل");
        }
    }
}
