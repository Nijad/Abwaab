using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Infrastructure.Presistence.Context;

namespace Abwaab.Infrastructure.Presistence.Repositories
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly AppDbContext _context;

        public PropertyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateProperty(Property property)
        {
            _context.Properties.Add(property);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetPropertiesCountBelongToPlanAsync(Guid planId)
        {
            return _context.Properties.Where(x=>x.UserPlandId == planId).Count();
        }
    }
}
