using FluentValidation;

namespace Abwaab.Application.Features.Properties.Unstar
{
    public class UnstarPropertyCommandValidation : AbstractValidator<UnstarPropertyCommand>
    {
        public UnstarPropertyCommandValidation()
        {
            RuleFor(x=>x.PropertyId)
                .NotEmpty().WithMessage("رقم تعريف العقار مطلوب.");
        }
    }
}
