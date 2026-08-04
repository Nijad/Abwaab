using Abwaab.Domain.Entities.UserEntities;

namespace Abwaab.Application.Repositories
{
    public interface IPlanRepository
    {
        Task AssignPlanToUserAsync(Guid userId, Guid planId);
        Task<Plan?> GetDefaultPlanAsync();
        Task<bool> UserHasActivePlanAsync(Guid id);
    }
}
