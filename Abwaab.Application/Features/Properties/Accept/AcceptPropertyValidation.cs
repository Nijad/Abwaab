using FluentValidation;

namespace Abwaab.Application.Features.Properties.Accept;

public class AcceptPropertyValidation : AbstractValidator<AcceptPropertyCommand>
{
    public AcceptPropertyValidation()
    {
        RuleFor(x => x.PropertyId)
            .NotEmpty().WithMessage("رقم العقار مطلوب");
    }
}
