using FluentValidation;

namespace Abwaab.Application.Features.Plans.CancelPlan
{
    public class CancelUserPlanVCommandalidation : AbstractValidator<CancelUserPlanCommand>
    {
        public CancelUserPlanVCommandalidation()
        {
            RuleFor(x => x.UserPlanId)
                .NotEmpty().WithMessage("Plan Id is required");
        }
    }
}
