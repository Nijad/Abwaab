using FluentValidation;

namespace Abwaab.Application.Features.Properties.Enable;

public class EnablePropertyValidation : AbstractValidator<EnablePropertyCommand>
{
    public EnablePropertyValidation()
    {
        RuleFor(x => x.PropertyId)
            .NotEmpty().WithMessage("رقم العقار مطلوب");
    }
}
