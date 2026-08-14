using Abwaab.Application.Common.Enums;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Contracts;
using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.UserEntities;

namespace Abwaab.Infrastructure.Services
{
    public class UserPlanStateService : IUserPlanStateService
    {
        private readonly IPlanRepository _planRepository;

        public UserPlanStateService(IPlanRepository planRepository)
        {
            _planRepository = planRepository;
        }

        public async Task<UserPlanStatus> FindUserPlanStatusByStatusNameAsync(string statusName, string errorTitle)
        {
            UserPlanStatus? userPlanStatus = await _planRepository.FindUserPlanStatusByNameAsync(statusName);

            if (userPlanStatus == null)
                throw new NotFoundException(nameof(UserPlanStatus), nameof(userPlanStatus.StateName), statusName, errorTitle);

            return userPlanStatus;
        }

        public async Task<UserPlanStatus> GetActiveUserPlanStatus(string errorTitle)
        {
            string statusName = UserPlanStatesEnum.Active.ToString();
            return await FindUserPlanStatusByStatusNameAsync(statusName, errorTitle);
        }

        public async Task<UserPlanStatus> GetWorkingUserPlanStatus(string errorTitle)
        {
            string statusName = UserPlanStatesEnum.Working.ToString();
            return await FindUserPlanStatusByStatusNameAsync(statusName, errorTitle);
        }

        public async Task<UserPlanStatus> GetPendingUserPlanStatus(string errorTitle)
        {
            string statusName = UserPlanStatesEnum.Pending.ToString();
            return await FindUserPlanStatusByStatusNameAsync(statusName, errorTitle);
        }

        public async Task<UserPlanStatus> GetExpieredUserPlanStatus(string errorTitle)
        {
            string statusName = UserPlanStatesEnum.Expiered.ToString();
            return await FindUserPlanStatusByStatusNameAsync(statusName, errorTitle);
        }

        public async Task<UserPlanStatus> GetCanceledUserPlanStatus(string errorTitle)
        {
            string statusName = UserPlanStatesEnum.Canceled.ToString();
            return await FindUserPlanStatusByStatusNameAsync(statusName, errorTitle);
        }
    }
}
