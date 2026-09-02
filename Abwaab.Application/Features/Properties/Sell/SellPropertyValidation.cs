using FluentValidation;

namespace Abwaab.Application.Features.Properties.Sell;

public class SellPropertyValidation : AbstractValidator<SellPropertyCommand>
{
    public SellPropertyValidation()
    {
        RuleFor(x => x.PropertyId)
            .NotEmpty().WithMessage("رقم العقار مطلوب.");
    }
}
