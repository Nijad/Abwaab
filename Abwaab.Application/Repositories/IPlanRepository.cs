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
        Task<UserPlanStatus?> FindUserPlanStatusByNameAsync(string statusName);
        Task<bool> UserHasPlanAsync(Guid userId ,Guid planId, string errorTitle);
        Task<Guid> GetUserPlanStateIdAsync(UserPlanStatesEnum state, string errorTitle);
        Task ActiveUserPlanAsync(Guid userId, Guid planId, string errorTitle);
        Task UpdateUserPlanAsync(UserPlan userPlan);
        Task<List<UserPlan>> FindUserPlansByStatusAsync(Guid userId, Guid stateId);
        Task<UserPlan?> FindUserPlanByIdAsync(Guid planId);
        Task AddUserPlanAsync(UserPlan userPlan);
    }
}
