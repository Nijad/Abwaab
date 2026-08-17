using FluentValidation;

namespace Abwaab.Application.Features.Properties.Update.BasicInfo
{
    public class UpdatePropertyBasicInfoValidation : AbstractValidator<UpdatePropertyBasicInfoCommand>
    {
        public UpdatePropertyBasicInfoValidation()
        {
            RuleFor(x=>x.PropertyId)
                .NotEmpty().WithMessage("رقم العقار مطلوب");
        }
    }
}
