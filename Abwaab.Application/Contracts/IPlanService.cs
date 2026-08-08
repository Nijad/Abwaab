using Abwaab.Application.Common.Enums;
using Abwaab.Domain.Entities.UserEntities;

namespace Abwaab.Application.Contracts
{
    public interface IPlanService
    {
        Task<UserPlan?> FindUserPlanByIdAsync(Guid planId);
        Task<bool> IsPendingUserPlanAsync(UserPlan userPlan);
        Task<bool> IsUserPlanBelongToUserAsync(Guid userPlanId, Guid userId);
        Task<UserPlanStatus> FindUserPlanStatusByStatusNameAsync(UserPlanStatesEnum statusName);
        Task UpdateUserPlan(UserPlan userPlan);
        Task ActivatePlan(UserPlan? userPlan);
    }
}
