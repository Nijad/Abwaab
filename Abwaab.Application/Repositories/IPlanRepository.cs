using Abwaab.Domain.Entities.UserEntities;

namespace Abwaab.Application.Repositories
{
    public interface IPlanRepository
    {
        Task AssignPlanToUserAsync(Guid userId, Guid planId);
        Task<Plan?> GetDefaultPlanAsync();
        Task<Plan?> GetPlanByIdAsync(Guid planId);
        Task UpgradeUserPlanAsync(ApplicationUser user, Plan plan);
        Task<bool> UserHasActivePlanAsync(Guid id);
    }
}
