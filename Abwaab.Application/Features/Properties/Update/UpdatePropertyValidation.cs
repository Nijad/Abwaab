using FluentValidation;

namespace Abwaab.Application.Features.Properties.Update
{
    public class UpdatePropertyValidation : AbstractValidator<UpdatePropertyCommand>
    {
        public UpdatePropertyValidation()
        {
            RuleFor(x=>x.PropertyId)
                .NotEmpty().WithMessage("رقم العقار مطلوب");
        }
    }
}
