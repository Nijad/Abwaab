using Abwaab.Application.Contracts;
using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.UserEntities;

namespace Abwaab.Infrastructure.Services
{
    public class PlanService : IPlanService
    {
        private readonly IPlanRepository _planRepository;
        private readonly IUserPlanStateService _userPlanStateService;

        public PlanService(
            IPlanRepository planRepository, 
            IUserPlanStateService userPlanStateService)
        {
            _planRepository = planRepository;
            _userPlanStateService = userPlanStateService;
        }

        public async Task<UserPlan?> FindUserPlanByIdAsync(Guid planId)
        {
            UserPlan? userPlan = await _planRepository.FindUserPlanByIdAsync(planId);
            return userPlan;
        }

        public async Task<bool> IsPendingUserPlanAsync(UserPlan userPlan)
        {
            UserPlanStatus pendingUserPlanStatus = await _userPlanStateService.GetPendingUserPlanStatus();

            return pendingUserPlanStatus.Id == userPlan.UserPlanStateId;
        }

        public Task<bool> IsUserPlanBelongToUserAsync(Guid userPlanId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateUserPlan(UserPlan userPlan)
        {
            await _planRepository.UpdateUserPlanAsync(userPlan);
        }

        public async Task ActivatePlan(UserPlan userPlan)
        {
            UserPlanStatus? activeUserPlanState = await _userPlanStateService.GetActiveUserPlanStatus();

            UserPlanStatus? workingUserPlanState = await _userPlanStateService.GetWorkingUserPlanStatus();

            //find active plan if exist and change status to working
            UserPlan? activePlan = await FindUserActivePlanAsync(userPlan.UserId);
            
            if (activePlan != null)
            {
                activePlan.UserPlanStatus = workingUserPlanState;
                activePlan.UserPlanStateId = workingUserPlanState.Id;
                await UpdateUserPlan(activePlan);
            }

            userPlan!.UserPlanStateId = activeUserPlanState!.Id;
            userPlan.UserPlanStatus = activeUserPlanState;
            await UpdateUserPlan(userPlan);
        }

        public async Task<UserPlan?> FindUserActivePlanAsync(Guid userId)
        {
            return await _planRepository.FindUserActivePlanAsync(userId);
        }
    }
}
