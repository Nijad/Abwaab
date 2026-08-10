using Abwaab.Application.Common.Enums;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.PaymentEntities;
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

        public async Task<bool> CheckIfUserHasActivePlan(Guid userId)
        {
            Guid activeUserPlanStateId = await GetUserPlanStateId(UserPlanStatesEnum.Active);

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

        public async Task<UserPlanStatus?> FindUserPlanStatusByNameAsync(string palnName)
        {
            UserPlanStatus? userPlanStatus = await _context.UserPlansStatus.Where(x => x.StateName == palnName).FirstOrDefaultAsync();

            return userPlanStatus;
        }

        public async Task UpgradeUserPlanAsync(ApplicationUser user, Plan plan)
        {
            PaymentState? paymentState = await _context.PaymentStates.FirstOrDefaultAsync(ps => ps.StateName == PaymentStatesEnum.Pending.ToString());

            if (paymentState == null)
                throw new NotFoundException("PaymentSatate", nameof(PaymentState.StateName), PaymentStatesEnum.Pending.ToString());

            ServiceType? serviceType = await _context.ServiceTypes.FirstOrDefaultAsync(st => st.ServiceName == ServiceTypesEnum.Plan_Subscription.ToString().Replace("_", " "));

            if (serviceType == null)
                throw new NotFoundException("ServiceType", nameof(ServiceType.ServiceName), ServiceTypesEnum.Plan_Subscription.ToString().Replace("_", " "));

            string userPlanStatesName = UserPlanStatesEnum.Pending.ToString();
            UserPlanStatus? userPlanStatus = await FindUserPlanStatusByNameAsync(userPlanStatesName);

            if (userPlanStatus == null)
                throw new NotFoundException("UserPlanStatus", nameof(userPlanStatus.StateName), userPlanStatesName);

            await _context.UserPlans.AddAsync(new UserPlan
            {
                Id = new Guid(),
                User = user,
                UserId = user.Id,
                Plan = plan,
                PlanId = plan.Id,
                UserPlanStatus = userPlanStatus,
                UserPlanStateId = userPlanStatus.Id,
                SubscriptionDate = DateOnly.FromDateTime(DateTime.Today),
                CreatedAt = DateTime.UtcNow,
                CreatedBy = $"{user.FirstName} {user.LastName}",
                Payments = new List<Payment>
                {
                    new Payment
                    {
                        Id = new Guid(),
                        Amount = plan.Price,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = $"{user.FirstName} {user.LastName}",
                        Description = $"Payment for upgrading to {plan.Name} plan",
                        PaymentCode = Guid.NewGuid().ToString(),
                        PaymentState = paymentState,
                        PaymentStateId = paymentState.Id,
                        ServiceType = serviceType,
                        ServiceTypeId = serviceType.Id
                    }
                }
            });

            await _context.SaveChangesAsync();
        }

        public async Task<bool> UserHasPlan(Guid userId, Guid planId)
        {
            Guid activUserPlanStateId = await GetUserPlanStateId(UserPlanStatesEnum.Active);
            Guid pendingUserPlanStateId = await GetUserPlanStateId(UserPlanStatesEnum.Pending);

            return await _context.UserPlans.AnyAsync(
                x => x.UserId == userId &&
                x.PlanId == planId && (
                    x.UserPlanStateId == activUserPlanStateId ||
                    x.UserPlanStateId == pendingUserPlanStateId));
        }

        public async Task<Guid> GetUserPlanStateId(UserPlanStatesEnum state)
        {
            UserPlanStatus? userPlanState = await _context.UserPlansStatus.Where(x => x.StateName == state.ToString()).FirstOrDefaultAsync();

            if (userPlanState == null)
                throw new NotFoundException(nameof(UserPlanStatus), nameof(userPlanState.StateName), UserPlanStatesEnum.Active.ToString());

            return userPlanState.Id;
        }

        public async Task ActiveUserPlan(Guid userId, Guid planId)
        {
            Guid userActivePlanStateId = await GetUserPlanStateId(UserPlanStatesEnum.Active);
            UserPlan? currentActiveUserPlan = await _context.UserPlans.Where(x => x.UserId == userId && x.UserPlanStateId == userActivePlanStateId).FirstOrDefaultAsync();

            if (currentActiveUserPlan != null)
            {
                // change user plan state to working
                Guid useWorkingPlanStateId = await GetUserPlanStateId(UserPlanStatesEnum.Working);

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

        public async Task<UserPlan?> FindUserActivePlanAsync(Guid userId)
        {
            //todo need checking
            string stateName = UserPlanStatesEnum.Active.ToString();
            UserPlanStatus? activeUserPlanState = await FindUserPlanStatusByNameAsync(stateName);

            if(activeUserPlanState == null)
                throw new NotFoundException(nameof(UserPlanStatus), nameof(UserPlanStatus.StateName), stateName);

            UserPlan? userPlan = await _context.UserPlans.Where(x=>x.UserId==userId && x.UserPlanStateId == activeUserPlanState.Id).FirstOrDefaultAsync();

            return userPlan;
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
    }
}
