using MediatR;

namespace Abwaab.Application.Features.Users.Profile.UserPlans.Cancel
{
    public class CancelUserPlanCommand : IRequest<CancelUserPlanResponse>
    {
        public Guid UserPlanId { get; set; }
    }
}
