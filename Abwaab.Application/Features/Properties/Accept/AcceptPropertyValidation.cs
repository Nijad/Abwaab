using FluentValidation;

namespace Abwaab.Application.Features.Properties.Accept;

public class DisablePropertyValidation : AbstractValidator<DisablePropertyCommand>
{
    public DisablePropertyValidation()
    {
        RuleFor(x => x.PropertyId)
            .NotEmpty().WithMessage("رقم العقار مطلوب");
    }
}
