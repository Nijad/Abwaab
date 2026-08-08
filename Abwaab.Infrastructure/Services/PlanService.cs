using Abwaab.Application.Common.Enums;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Contracts;
using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Infrastructure.Presistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Abwaab.Infrastructure.Services
{
    public class PlanService : IPlanService
    {
        private readonly AppDbContext _context;
        private readonly IPlanRepository _planRepository;

        public PlanService(AppDbContext context, IPlanRepository planRepository)
        {
            _context = context;
            _planRepository = planRepository;
        }

        public async Task<UserPlan?> FindUserPlanByIdAsync(Guid planId)
        {
            return await _context.UserPlans.Include(x => x.Payments).Where(x => x.Id == planId).FirstOrDefaultAsync();
        }

        public async Task<bool> IsPendingUserPlanAsync(UserPlan userPlan)
        {
            UserPlanStatus pendingUserPlanStatus = await FindUserPlanStatusByStatusNameAsync(UserPlanStatesEnum.Pending);

            return pendingUserPlanStatus.Id == userPlan.UserPlanStateId;
        }

        public async Task<UserPlanStatus> FindUserPlanStatusByStatusNameAsync(UserPlanStatesEnum statusName)
        {
            UserPlanStatus? status = await _context.UserPlansStatus.Where(x => x.StateName == statusName.ToString()).FirstOrDefaultAsync();

            if (status == null)
                throw new NotFoundException(nameof(UserPlanStatus), status.StateName, statusName.ToString());

            return status;
        }

        public Task<bool> IsUserPlanBelongToUserAsync(Guid userPlanId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateUserPlan(UserPlan userPlan)
        {
            _context.UserPlans.Update(userPlan);
            await _context.SaveChangesAsync();
        }

        public async Task ActivatePlan(UserPlan? userPlan)
        {
            UserPlanStatus? activeUserPlanState = await _planRepository.FindUserPlanStatusByNameAsync(UserPlanStatesEnum.Active.ToString());

            UserPlanStatus? workingUserPlanState = await _planRepository.FindUserPlanStatusByNameAsync(UserPlanStatesEnum.Working.ToString());

            //find active plan if exist and change status to working
            UserPlan? activePlan = await _planRepository.FindUserActivePlanAsync(userPlan?.UserId);
            
            if (activePlan != null)
            {
                activePlan.UserPlanStatus = workingUserPlanState;
                activePlan.UserPlanStateId = workingUserPlanState.Id;
                await UpdateUserPlan(activePlan);
            }

            userPlan!.UserPlanStateId = activeUserPlanState!.Id;
            userPlan.UserPlanStatus = activeUserPlanState;
            await UpdateUserPlan(userPlan);
        }
    }
}
