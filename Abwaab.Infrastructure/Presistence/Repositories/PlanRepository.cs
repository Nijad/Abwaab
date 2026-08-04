using Abwaab.Application.Repositories;
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

        public Task<Plan?> GetDefaultPlanAsync()
        {
            return _context.Plans.FirstOrDefaultAsync(p => p.DefaultPlan == true);
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
    }
}
