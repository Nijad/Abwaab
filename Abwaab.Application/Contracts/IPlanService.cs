using Abwaab.Domain.Entities.UserEntities;

namespace Abwaab.Application.Contracts
{
    public interface IPlanService
    {
        Task<UserPlan?> FindUserPlanByIdAsync(Guid planId);
        Task<bool> IsPendingUserPlan(UserPlan userPlan);
    }
}
