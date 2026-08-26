using FluentValidation;

namespace Abwaab.Application.Features.Properties.Star
{
    public class StarPropertyCommandValidation : AbstractValidator<StarPropertyCommand>
    {
        public StarPropertyCommandValidation()
        {
            RuleFor(x=>x.PropertyId)
                .NotEmpty().WithMessage("رقم تعريف العقار مطلوب.");
        }
    }
}
