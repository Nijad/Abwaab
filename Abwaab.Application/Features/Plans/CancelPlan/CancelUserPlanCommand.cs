using MediatR;

namespace Abwaab.Application.Features.Plans.CancelPlan
{
    public class CancelUserPlanCommand : IRequest<CancelUserPlanResponse>
    {
        public Guid UserPlanId { get; set; }
    }
}
