using FluentValidation;

namespace Abwaab.Application.Features.Properties.Delete;

public class DeletePropertyValidation : AbstractValidator<DeletePropertyCommand>
{
    public DeletePropertyValidation()
    {
        RuleFor(x => x.PropertyId)
            .NotEmpty().WithMessage("رقم العقار مطلوب.");
    }
}
