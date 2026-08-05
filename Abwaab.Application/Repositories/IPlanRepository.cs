using Abwaab.Application.Features.Plans.GetAllPlans;
using Abwaab.Domain.Entities.UserEntities;

namespace Abwaab.Application.Repositories
{
    public interface IPlanRepository
    {
        Task AddPlanAsync(Plan plan);
        Task AssignPlanToUserAsync(Guid userId, Guid planId);
        Task<List<Plan>> GetAllAsync();
        Task<Plan?> GetDefaultPlanAsync();
        Task<Plan?> GetPlanByIdAsync(Guid planId);
        Task UpgradeUserPlanAsync(ApplicationUser user, Plan plan);
        Task<bool> UserHasActivePlanAsync(Guid id);
        Task<bool> UserHasPlan(Guid userId ,Guid planId);
    }
}
