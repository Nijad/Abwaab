using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Common.Exceptions.Plans;
using Abwaab.Application.Common.Exceptions.Profile.Plans;
using Abwaab.Application.Contracts;
using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Abwaab.Application.Features.Users.Profile.UserPlans.Upgrade
{
    public class UpgradeUserPlanCommandHandler : IRequestHandler<UpgradeUserPlanComman, UpgradeUserPlanResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPlanRepository _planRepository;
        private readonly IUserService _userService;
        private readonly string errorTitle = ErrorTitle.UpgradeUserPlan;

        public UpgradeUserPlanCommandHandler(UserManager<ApplicationUser> userManager, IPlanRepository planRepository, IUserService userService)
        {
            _userManager = userManager;
            _planRepository = planRepository;
            _userService = userService;
        }

        public async Task<UpgradeUserPlanResponse> Handle(UpgradeUserPlanComman request, CancellationToken cancellationToken)
        {
            string? username = _userService.FindUserNameByContext();
            if(username == null)
                throw new NotFoundException(
                    entity: "user context",
                    property: "username",
                    value: "",
                    title: errorTitle,
                    returnToUser: false);

            ApplicationUser? user = await _userManager.FindByNameAsync(username);
            if(user == null)
                throw new UserNotFoundException(username, errorTitle);

            Plan? plan = await _planRepository.GetPlanByIdAsync(request.PlanId);

            if (plan == null)
                throw new NotFoundException("Plan", nameof(request.PlanId), request.PlanId.ToString(), errorTitle);

            // check if plan is disabled or expired
            if (plan.IsDisabled || plan.ExpieryDate < DateOnly.FromDateTime(DateTime.UtcNow))
                throw new PlanNotAvailableException(errorTitle);

            // check if the user already has the plan
            bool userAlreadyHasPlan = await _planRepository.UserHasPlan(user.Id, plan.Id, errorTitle);
            if (userAlreadyHasPlan)
                throw new UserAlreadyHasPlanException(errorTitle);

            await _planRepository.UpgradeUserPlanAsync(user, plan, errorTitle);

            return new UpgradeUserPlanResponse
            {
                Success = true,
                Message = "Plan upgraded successfully"
            };
        }
    }
}
