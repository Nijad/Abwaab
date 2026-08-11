using MediatR;

namespace Abwaab.Application.Features.Users.Profile.UserPlans.Upgrade
{
    public class UpgradeUserPlanComman: IRequest<UpgradeUserPlanResponse>
    {
        public Guid PlanId { get; set; }
    }
}
