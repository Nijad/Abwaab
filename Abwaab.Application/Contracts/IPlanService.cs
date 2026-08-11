using Abwaab.Domain.Entities.UserEntities;

namespace Abwaab.Application.Contracts
{
    public interface IPlanService
    {
        Task<UserPlan?> FindUserPlanByIdAsync(Guid planId);
        Task<bool> IsUserPlanHasStatusAsync(UserPlan userPlan, UserPlanStatus status);
        Task<bool> IsUserPlanBelongToUserAsync(Guid userPlanId, Guid userId);
        Task UpdateUserPlan(UserPlan userPlan);
        Task<UserPlan> FindUserActivePlanAsync(Guid userId, Guid activeUserPlanStateId);
    }
}
