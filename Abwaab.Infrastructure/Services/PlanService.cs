using Abwaab.Application.Common.Enums;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Contracts;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Infrastructure.Presistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Abwaab.Infrastructure.Services
{
    public class PlanService : IPlanService
    {
        private readonly AppDbContext _context;

        public PlanService(AppDbContext context)
        {
            _context = context;
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
            UserPlanStatus? status = await _context.UserPlansStatus.Where(x=>x.StateName == statusName.ToString()).FirstOrDefaultAsync();
            
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
    }
}
