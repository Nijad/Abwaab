using Abwaab.Application.Common.Enums;
using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.PaymentEntities;
using Abwaab.Infrastructure.Presistence.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Abwaab.Infrastructure.Presistence.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string actionBy;

        public PaymentRepository(
            AppDbContext context, 
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            actionBy = _httpContextAccessor!.HttpContext!.User!.Identity!.Name!;
        }

        public async Task<ServiceType?> FindServiceTypeByName(ServiceTypesEnum plan_Subscription)
        {
            return await _context.ServiceTypes.Where(x => x.ServiceName == plan_Subscription.ToString()).FirstOrDefaultAsync();
        }
    }
}
