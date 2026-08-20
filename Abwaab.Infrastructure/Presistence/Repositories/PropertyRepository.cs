using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Infrastructure.Presistence.Context;
using Microsoft.EntityFrameworkCore;
using Attribute = Abwaab.Domain.Entities.PropertyEntities.Attribute;

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
                .Where(x => x.Id == propertyId)
                .FirstOrDefaultAsync();
        }

        public async Task<Property?> FindPropertyByIdForUpdateAsync(Guid propertyId)
        {
            return await _context.Properties
                .Include(p => p.UserPlan)
                .Include(p => p.PropertyType)
                .Include(p => p.Finishing)
                .Include(p => p.TimeSlots)
                .Include(p => p.PropertyAttributes)
                .ThenInclude(x=>x.Attribute)
                .ThenInclude(x=>x.AttributeDataType)
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

        public async Task<List<Attribute>> GetAttributesListAsync()
        {
            return await _context.Attributes
                .Include(x => x.AttributeDataType)
                .Include(x => x.PossibleValues)
                .ToListAsync();
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

        public async Task<List<TimeSlot>> GetTimeSlotsByPropertyIdAsync(Guid propertyId)
        {
            return await _context.TimeSlots.Where(x => x.PropertyId == propertyId).ToListAsync();
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
