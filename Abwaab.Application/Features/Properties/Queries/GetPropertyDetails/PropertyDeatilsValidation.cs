using FluentValidation;

namespace Abwaab.Application.Features.Properties.Queries.GetPropertyDetails;

public class PropertyDeatilsValidation : AbstractValidator<PropertyDetailsQuery>
{
    public PropertyDeatilsValidation()
    {
        RuleFor(x=>x.PropertyId)
            .NotEmpty().WithMessage("رقم العقار مطلوب");
    }
}
