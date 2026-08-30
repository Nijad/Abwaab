using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Infrastructure.Presistence.Context;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Property?> FindPropertyByIdAsync(Guid propertyId)
        {
            return await _context.Properties
                .Include(x => x.PropertyState)
                .Include(x => x.UserPlan)
                .ThenInclude(x => x.Plan)
                .Include(p => p.TimeSlots)
                .Include(p => p.PropertyAttributes)
                //.ThenInclude(x=>x.Attribute)
                //.ThenInclude(x=>x.AttributeDataType)
                .Where(x => x.Id == propertyId)
                .FirstOrDefaultAsync();
        }

        public async Task<Property?> FindPropertyByIdForUpdateAsync(Guid propertyId)
        {
            return await _context.Properties
                .Include(p => p.UserPlan)
                .ThenInclude(x => x.Properties)
                .Include(p => p.UserPlan)
                .ThenInclude(x => x.Plan)
                .Include(p => p.PropertyType)
                .Include(p => p.Finishing)
                .Include(p => p.TimeSlots)
                .Include(p => p.PropertyAttributes)
                .ThenInclude(x => x.Attribute)
                .ThenInclude(x => x.AttributeDataType)
                .Include(x => x.MediaList)
                .ThenInclude(x => x.MediaType)
                .Include(x=>x.PropertyState)
                .Where(p => p.Id == propertyId)
                .FirstOrDefaultAsync();
        }

        public async Task<Finishing?> FindPropertyFinishingByIdAsync(Guid finishingId)
        {
            return await _context.Finishings.Where(x => x.Id == finishingId).FirstOrDefaultAsync();
        }

        public async Task<PropertyState?> FindPropertyStateByStateNameAsync(string propertyStateName)
        {
            return await _context.PropertyStates.Where(x => x.StateName == propertyStateName).FirstOrDefaultAsync();
        }

        public async Task<PropertyType?> FindPropertyTypeByIdAsync(Guid propertyTypeId)
        {
            return await _context.PropertyTypes.Where(x => x.Id == propertyTypeId).FirstOrDefaultAsync();
        }

        public async Task<Property?> FindPropertyWithUserAndStateByIdAsync(Guid propertyId)
        {
            return await _context.Properties
                .Include(x=>x.UserPlan)
                .ThenInclude(x=>x.User)
                .Include(x=>x.PropertyState)
                .Where(x => x.Id == propertyId)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetPropertiesCountBelongToPlanAsync(Guid planId)
        {
            return _context.Properties.Where(x => x.UserPlandId == planId).Count();
        }

        public async Task<List<Finishing>> GetPropertyFinishingListAsync()
        {
            return await _context.Finishings.ToListAsync();
        }

        public async Task<List<PropertyType>> GetProperyTypesList()
        {
            return await _context.PropertyTypes.ToListAsync();
        }

        public async Task<int> GetStaredPropertyCountInPlanAsync(Guid userPlandId)
        {
            return await _context.Properties
                .Where(x => x.UserPlandId == userPlandId && x.IsStard)
                .CountAsync();
        }

        public async Task<List<Property>> GetUserPropertiesList(Guid userId)
        {
            return await _context.Properties
                .Include(x => x.PropertyType)
                .Include(x => x.Finishing)
                .Include(x => x.MediaList.Where(y=>y.IsCover))
                .Include(x=>x.PropertyState)
                .Where(x => x.UserPlan.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> PropertyBelongToUser(Guid userId, Guid propertyId)
        {
            Property property = await _context.Properties.Include(x => x.UserPlan).Where(x => x.Id == propertyId).FirstAsync();
            return property.UserPlan.UserId == userId;
        }

        public async Task UpdatePropertyAsync(Property property)
        {
            _context.Properties.Update(property);
            await _context.SaveChangesAsync();
        }
    }
}
