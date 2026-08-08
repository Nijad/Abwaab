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

        public Task<bool> IsPendingUserPlan(UserPlan userPlan)
        {

            throw new NotImplementedException();
        }
    }
}
