using Abwaab.Domain.Entities.UserEntities;

namespace Abwaab.Application.Contracts
{
    public interface IUserPlanStateService
    {
        Task<UserPlanStatus> FindUserPlanStatusByStatusNameAsync(string statusName, string errorTitle);
        Task<UserPlanStatus> GetActiveUserPlanStatus(string errorTitle);
        Task<UserPlanStatus> GetWorkingUserPlanStatus(string errorTitle);
        Task<UserPlanStatus> GetPendingUserPlanStatus(string errorTitle);
        Task<UserPlanStatus> GetExpieredUserPlanStatus(string errorTitle);
        Task<UserPlanStatus> GetCanceledUserPlanStatus(string errorTitle);
    }
}
