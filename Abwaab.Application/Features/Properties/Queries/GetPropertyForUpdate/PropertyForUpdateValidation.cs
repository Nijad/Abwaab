using FluentValidation;

namespace Abwaab.Application.Features.Properties.Queries.GetPropertyForUpdate
{
    public class PropertyForUpdateValidation : AbstractValidator<PropertyForUpdateQuery>
    {
        public PropertyForUpdateValidation()
        {
            RuleFor(x => x.PropertyId)
                .NotEmpty().WithMessage("رقم العقار مطلوب");
        }
    }
}
