using MediatR;

namespace Abwaab.Application.Features.Users.Profile.Plans.Cancel
{
    public class CancelPlanCommand : IRequest<CancelPlanResponse>
    {
        public Guid PlanId { get; set; }
    }
}
