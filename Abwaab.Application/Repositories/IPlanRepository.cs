using Abwaab.Application.Common.Enums;
using Abwaab.Domain.Entities.UserEntities;

namespace Abwaab.Application.Repositories
{
    public interface IPlanRepository
    {
        Task AddPlanAsync(Plan plan);
        Task AssignPlanToUserAsync(Guid userId, Guid planId, Guid userPlansStateId);
        Task<bool> CheckIfUserHasActivePlan(Guid userId);
        Task<List<Plan>> GetAllAsync();
        Task<Plan?> GetDefaultPlanAsync();
        Task<Plan?> GetPlanByIdAsync(Guid planId);
        Task<UserPlanStatus?> GetUserPlanStatusByNameAsync(string planName);
        Task UpgradeUserPlanAsync(ApplicationUser user, Plan plan);
        Task<bool> UserHasPlan(Guid userId ,Guid planId);
        Task<Guid> GetUserPlanStateId(UserPlanStatesEnum state);
        Task ActiveUserPlan(Guid userId, Guid planId);
    }
}
