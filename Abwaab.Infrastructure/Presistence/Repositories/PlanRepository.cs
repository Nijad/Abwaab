using Abwaab.Application.Common.Enums;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.PaymentEntities;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Infrastructure.Presistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Abwaab.Infrastructure.Presistence.Repositories
{

    public class PlanRepository : IPlanRepository
    {
        private readonly AppDbContext _context;

        public PlanRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddPlanAsync(Plan plan)
        {
            await _context.Plans.AddAsync(plan);
            await _context.SaveChangesAsync();
        }

        public async Task AssignPlanToUserAsync(Guid userId, Guid planId)
        {
            UserPlan userPlan = new UserPlan
            {
                Id = new Guid(),
                UserId = userId,
                PlanId = planId,
                SubscriptionDate = DateOnly.FromDateTime(DateTime.Today),
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            };

            _context.UserPlans.Add(userPlan);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Plan>> GetAllAsync()
        {
            return  await _context.Plans.ToListAsync();
        }

        public async Task<Plan?> GetDefaultPlanAsync()
        {
            return await _context.Plans.FirstOrDefaultAsync(p => p.DefaultPlan == true);
        }

        public async Task<Plan?> GetPlanByIdAsync(Guid planId)
        {
            return await _context.Plans.FirstOrDefaultAsync(p => p.Id == planId);
        }

        public async Task UpgradeUserPlanAsync(ApplicationUser user, Plan plan)
        {
            PaymentState? paymentState = await _context.PaymentStates.FirstOrDefaultAsync(ps => ps.StateName == PaymentStatesEnum.Pending.ToString());

            if (paymentState == null)
                throw new NotFoundException("PaymentSatate", nameof(PaymentState.StateName), PaymentStatesEnum.Pending.ToString());
            
            ServiceType? serviceType = await _context.ServiceTypes.FirstOrDefaultAsync(st => st.ServiceName == ServiceTypesEnum.Plan_Subscription.ToString().Replace("_", " "));

            if (serviceType == null)
                throw new NotFoundException("ServiceType", nameof(ServiceType.ServiceName), ServiceTypesEnum.Plan_Subscription.ToString().Replace("_", " "));

            await _context.UserPlans.AddAsync(new UserPlan
            {
                Id = new Guid(),
                User = user,
                UserId = user.Id,
                Plan = plan,
                PlanId = plan.Id,
                SubscriptionDate = DateOnly.FromDateTime(DateTime.Today),
                IsActive = false,
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

        public async Task<bool> UserHasActivePlanAsync(Guid id)
        {
            List<UserPlan> activeUserPlans = await _context.UserPlans.Where(
                up => up.UserId == id && 
                up.IsActive == true&&
                up.SubscriptionDate.AddDays(up.Plan.DurationInDays) >= DateOnly.FromDateTime(DateTime.Today))
                .ToListAsync();
            return activeUserPlans.Count > 0;
        }

        public Task<bool> UserHasPlan(Guid userId, Guid planId)
        {
            //_context.UserPlans.Where(x=>x.UserId == userId && x.PlanId==planId && x.)
            return Task.FromResult(true);
        }
    }
}
