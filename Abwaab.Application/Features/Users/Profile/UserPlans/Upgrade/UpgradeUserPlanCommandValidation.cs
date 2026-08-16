using FluentValidation;

namespace Abwaab.Application.Features.Users.Profile.UserPlans.Upgrade
{
    public class UpgradeUserPlanCommandValidation : AbstractValidator<UpgradeUserPlanComman>
    {
        public UpgradeUserPlanCommandValidation()
        {
            RuleFor(x => x.PlanId)
                .NotEmpty().WithMessage("الخطة مطلوبة.");
        }
    }
}
