using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Contracts;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;

namespace Abwaab.Application.Features.Users.Profile.Plans.Cancel
{
    public class CancelPlanCommandHandler : IRequestHandler<CancelPlanCommand, CancelPlanResponse>
    {
        private readonly IPlanService _planService;

        public CancelPlanCommandHandler(IPlanService planService)
        {
            _planService = planService;
        }

        public async Task<CancelPlanResponse> Handle(CancelPlanCommand request, CancellationToken cancellationToken)
        {
            // find user plan by Id
            UserPlan? userPlan = await _planService.FindUserPlanByIdAsync(request.PlanId);
            if (userPlan == null)
                throw new NotFoundException(nameof(UserPlan), nameof(userPlan.Id), request.PlanId.ToString());

            // check if it is pending
            bool isPendingUserPlan = await _planService.IsPendingUserPlan(userPlan);

            // change it's status to canceled

            // change payment states to canceled


            return new CancelPlanResponse() { Success = true, Message = "Plan Canceled Successfully" };
        }
    }
}
