using Abwaab.Application.Common.Enums;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Infrastructure.Presistence.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Abwaab.Infrastructure.Presistence.Repositories
{

    public class PlanRepository : IPlanRepository
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string actionBy;

        public PlanRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            actionBy = _httpContextAccessor!.HttpContext!.User!.Identity!.Name!;
        }

        public async Task AddPlanAsync(Plan plan)
        {
            plan.CreatedBy = actionBy;
            plan.CreatedAt = DateTime.Now;

            await _context.Plans.AddAsync(plan);
            await _context.SaveChangesAsync();
        }

        public async Task AssignPlanToUserAsync(Guid userId, Guid planId, Guid userPlansStateId)
        {
            UserPlan userPlan = new UserPlan
            {
                Id = new Guid(),
                UserId = userId,
                PlanId = planId,
                UserPlanStateId = userPlansStateId,
                SubscriptionDate = DateOnly.FromDateTime(DateTime.Today),
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            };

            _context.UserPlans.Add(userPlan);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckIfUserHasActivePlan(Guid userId, string errorTitle)
        {
            Guid activeUserPlanStateId = await GetUserPlanStateIdAsync(UserPlanStatesEnum.Active, errorTitle);

            return await _context.UserPlans.AnyAsync(x => x.UserId == userId && x.UserPlanStateId == activeUserPlanStateId);
        }

        public async Task<List<Plan>> GetAllAsync()
        {
            return await _context.Plans.ToListAsync();
        }

        public async Task<Plan?> GetDefaultPlanAsync()
        {
            return await _context.Plans.FirstOrDefaultAsync(p => p.DefaultPlan == true);
        }

        public async Task<Plan?> GetPlanByIdAsync(Guid planId)
        {
            return await _context.Plans.FirstOrDefaultAsync(p => p.Id == planId);
        }

        public async Task<UserPlanStatus?> FindUserPlanStatusByNameAsync(string statusName)
        {
            return await _context.UserPlansStatus
                .Where(x => x.StateName == statusName)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UserHasPlanAsync(Guid userId, Guid planId, string errorTitle)
        {
            Guid activUserPlanStateId = await GetUserPlanStateIdAsync(UserPlanStatesEnum.Active, errorTitle);
            Guid pendingUserPlanStateId = await GetUserPlanStateIdAsync(UserPlanStatesEnum.Pending, errorTitle);

            return await _context.UserPlans.AnyAsync(
                x => x.UserId == userId &&
                x.PlanId == planId && (
                    x.UserPlanStateId == activUserPlanStateId ||
                    x.UserPlanStateId == pendingUserPlanStateId));
        }

        public async Task<Guid> GetUserPlanStateIdAsync(UserPlanStatesEnum state, string errorTitle)
        {
            UserPlanStatus? userPlanState = await _context.UserPlansStatus.Where(x => x.StateName == state.ToString()).FirstOrDefaultAsync();

            if (userPlanState == null)
                throw new NotFoundException(nameof(UserPlanStatus), nameof(userPlanState.StateName), UserPlanStatesEnum.Active.ToString(), errorTitle);

            return userPlanState.Id;
        }

        public async Task ActiveUserPlanAsync(Guid userId, Guid planId, string errorTitle)
        {
            Guid userActivePlanStateId = await GetUserPlanStateIdAsync(UserPlanStatesEnum.Active, errorTitle);
            UserPlan? currentActiveUserPlan = await _context.UserPlans.Where(x => x.UserId == userId && x.UserPlanStateId == userActivePlanStateId).FirstOrDefaultAsync();

            if (currentActiveUserPlan != null)
            {
                // change user plan state to working
                Guid useWorkingPlanStateId = await GetUserPlanStateIdAsync(UserPlanStatesEnum.Working, errorTitle);

                currentActiveUserPlan.UserPlanStateId = useWorkingPlanStateId;
                currentActiveUserPlan.LastModifiedBy = actionBy;
                currentActiveUserPlan.LastModifiedAt = DateTime.Now;

                _context.UserPlans.Update(currentActiveUserPlan);
            }

            // update target plan states to active
            UserPlan userPlan = await _context.UserPlans.Where(x => x.UserId == userId && x.PlanId == planId).FirstAsync();
            userPlan.UserPlanStateId = userActivePlanStateId;
            userPlan.LastModifiedBy = actionBy;
            userPlan.LastModifiedAt = DateTime.Now;

            _context.UserPlans.Update(userPlan);

            await _context.SaveChangesAsync();
        }

        public async Task<List<UserPlan>> FindUserPlansByStatusAsync(Guid userId, Guid stateId)
        {
            return _context.UserPlans.Include(x=>x.Plan).Where(x=>x.UserId == userId && x.UserPlanStateId == stateId).ToList();
        }

        public async Task UpdateUserPlanAsync(UserPlan userPlan)
        {
            _context.UserPlans.Update(userPlan);
            await _context.SaveChangesAsync();
        }

        public async Task<UserPlan?> FindUserPlanByIdAsync(Guid planId)
        {
            return await _context.UserPlans.Include(x => x.Payments).Where(x => x.Id == planId).FirstOrDefaultAsync();
        }

        public async Task AddUserPlanAsync(UserPlan userPlan)
        {
            userPlan.CreatedBy = actionBy;
            userPlan.CreatedAt = DateTime.Now;
            await _context.UserPlans.AddAsync(userPlan);
            await UpdateUserPlanAsync(userPlan);
        }
    }
}
