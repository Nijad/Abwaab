using FluentValidation;

namespace Abwaab.Application.Features.Users.Profile.Plans.Cancel
{
    public class CancelPlanValidation : AbstractValidator<CancelPlanCommand>
    {
        public CancelPlanValidation()
        {
            RuleFor(x=>x.PlanId).NotEmpty().WithMessage("Plan Id is required");
        }
    }
}
