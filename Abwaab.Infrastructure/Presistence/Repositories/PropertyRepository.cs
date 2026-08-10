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

        public async Task CreateProperty(Guid id)
        {
            _context.Properties.Add(new Property() { Id = id });
            await _context.SaveChangesAsync();
        }
    }
}
