using FluentValidation;

namespace Abwaab.Application.Features.Users.Profile.Plans.Upgrade
{
    public class UpgradePlanCommandValidation : AbstractValidator<UpgradePlanComman>
    {
        public UpgradePlanCommandValidation()
        {
            RuleFor(x => x.PlanId)
                .NotEmpty().WithMessage("PlanId is required.");
        }
    }
}
