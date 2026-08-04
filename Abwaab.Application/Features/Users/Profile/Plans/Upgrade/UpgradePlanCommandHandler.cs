using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Plans;
using Abwaab.Application.Common.Exceptions.Profile.Plans;
using Abwaab.Application.Contracts;
using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Abwaab.Application.Features.Users.Profile.Plans.Upgrade
{
    public class UpgradePlanCommandHandler : IRequestHandler<UpgradePlanComman, UpgradePlanResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPlanRepository _planRepository;
        private readonly IUserService _userService;

        public UpgradePlanCommandHandler(UserManager<ApplicationUser> userManager, IPlanRepository planRepository, IUserService userService)
        {
            _userManager = userManager;
            _planRepository = planRepository;
            _userService = userService;
        }

        public async Task<UpgradePlanResponse> Handle(UpgradePlanComman request, CancellationToken cancellationToken)
        {
            string? username = _userService.FindUserByContext();
            if(username == null)
                throw new NotFoundException("User", "Username", "Not found in context");

            ApplicationUser? user = await _userManager.FindByNameAsync(username);
            if(user == null)
                throw new NotFoundException("User", "Username", username);

            Plan? plan = await _planRepository.GetPlanByIdAsync(request.PlanId);

            if (plan == null)
                throw new NotFoundException("Plan", nameof(request.PlanId), request.PlanId.ToString());

            // check if plan is disabled or expired
            if (plan.IsDisabled || plan.ExpieryDate < DateOnly.FromDateTime(DateTime.UtcNow))
                throw new PlanNotAvailableException();

            // check if the user already has the plan
            if (user.UserPlans != null && user.UserPlans.Any(up => up.PlanId == plan.Id))
                throw new UserAlreadyHasPlanException();

            await _planRepository.UpgradeUserPlanAsync(user, plan);

            return new UpgradePlanResponse
            {
                Success = true,
                Message = "Plan upgraded successfully"
            };
        }
    }
}
