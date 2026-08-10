using Abwaab.Application.Common.Exceptions.Plans;
using Abwaab.Application.Contracts;
using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.UserEntities;

namespace Abwaab.Infrastructure.Services
{
    public class PlanService : IPlanService
    {
        private readonly IPlanRepository _planRepository;

        public PlanService(IPlanRepository planRepository)
        {
            _planRepository = planRepository;
        }

        public async Task<UserPlan?> FindUserPlanByIdAsync(Guid planId)
        {
            UserPlan? userPlan = await _planRepository.FindUserPlanByIdAsync(planId);
            return userPlan;
        }

        public async Task<bool> IsUserPlanHasStatusAsync(UserPlan userPlan, UserPlanStatus status)
        {
            return status.Id == userPlan.UserPlanStateId;
        }

        public Task<bool> IsUserPlanBelongToUserAsync(Guid userPlanId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateUserPlan(UserPlan userPlan)
        {
            await _planRepository.UpdateUserPlanAsync(userPlan);
        }

        public async Task<UserPlan> FindUserActivePlanAsync(Guid userId, Guid activeUserPlanStateId)
        {
            List<UserPlan> userPlans = await _planRepository.FindUserPlansByStatusAsync(userId, activeUserPlanStateId);

            if (userPlans.Count == 0)
                throw new UserHasNoActivePlanException();

            if (userPlans.Count > 1)
                throw new UserHasMoreThanOneActivePlanException();

            return userPlans.First();
        }
    }
}
