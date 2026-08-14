using Abwaab.Application.Common.Enums;
using Abwaab.Domain.Entities.UserEntities;

namespace Abwaab.Application.Repositories
{
    public interface IPlanRepository
    {
        Task AddPlanAsync(Plan plan);
        Task AssignPlanToUserAsync(Guid userId, Guid planId, Guid userPlansStateId);
        Task<bool> CheckIfUserHasActivePlan(Guid userId, string errorTitle);
        Task<List<Plan>> GetAllAsync();
        Task<Plan?> GetDefaultPlanAsync();
        Task<Plan?> GetPlanByIdAsync(Guid planId);
        Task<UserPlanStatus> FindUserPlanStatusByNameAsync(string planName);
        Task UpgradeUserPlanAsync(ApplicationUser user, Plan plan, string errorTitle);
        Task<bool> UserHasPlan(Guid userId ,Guid planId, string errorTitle);
        Task<Guid> GetUserPlanStateId(UserPlanStatesEnum state, string errorTitle);
        Task ActiveUserPlan(Guid userId, Guid planId, string errorTitle);
        Task<UserPlan?> FindUserActivePlanAsync(Guid userId, string errorTitle);
        Task UpdateUserPlanAsync(UserPlan userPlan);
        Task<List<UserPlan>> FindUserPlansByStatusAsync(Guid userId, Guid stateId);
        Task<UserPlan?> FindUserPlanByIdAsync(Guid planId);
    }
}
