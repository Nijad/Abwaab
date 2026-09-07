using FluentValidation;

namespace Abwaab.Application.Features.Properties.Reject
{
    public class RejectPropertyValidation : AbstractValidator<RejectPropertyCommand>
    {
        public RejectPropertyValidation()
        {
            RuleFor(x => x.PropertyId)
                .NotEmpty().WithMessage("رقم العقار مطلوب");
        }
    }
}
