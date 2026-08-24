using Abwaab.Application.Features.Properties.Common.Validations;
using FluentValidation;

namespace Abwaab.Application.Features.Properties.Update
{
    public class UpdatePropertyValidation : AbstractValidator<UpdatePropertyCommand>
    {
        public UpdatePropertyValidation()
        {
            RuleFor(x=>x.PropertyId)
                .NotEmpty().WithMessage("رقم العقار مطلوب");

            RuleForEach(x => x.TimeSlots)
                .SetValidator(new TimeSlotDTOValidator());

            RuleForEach(x => x.PropertyAttributesList)
                .SetValidator(new PropertyAttributeDTOValidator())
                .When(x => x.PropertyAttributesList != null); 
        }
    }
}
