using MediatR;

namespace Abwaab.Application.Features.Users.Profile.Plans.Upgrade
{
    public class UpgradePlanComman: IRequest<UpgradePlanResponse>
    {
        public Guid PlanId { get; set; }
    }
}
