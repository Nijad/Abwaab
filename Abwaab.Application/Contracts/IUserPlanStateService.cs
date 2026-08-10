using Abwaab.Domain.Entities.UserEntities;

namespace Abwaab.Application.Contracts
{
    public interface IUserPlanStateService
    {
        Task<UserPlanStatus> FindUserPlanStatusByStatusNameAsync(string statusName);
        Task<UserPlanStatus> GetActiveUserPlanStatus();
        Task<UserPlanStatus> GetWorkingUserPlanStatus();
        Task<UserPlanStatus> GetPendingUserPlanStatus();
        Task<UserPlanStatus> GetExpieredUserPlanStatus();
        Task<UserPlanStatus> GetCanceledUserPlanStatus();
    }
}
