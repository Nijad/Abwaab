using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Infrastructure.Presistence.Context;
using Microsoft.EntityFrameworkCore;
using Attribute = Abwaab.Domain.Entities.PropertyEntities.Attribute;

namespace Abwaab.Infrastructure.Presistence.Repositories
{
    public class AttributeRepository : IAttributeRepository
    {
        private readonly AppDbContext _context;

        public AttributeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddPropertyAttribute(PropertyAttribute comming)
        {
            await _context.PropertyAttributes.AddAsync(comming);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePropertyAttributeAsync(PropertyAttribute existing)
        {
            _context.PropertyAttributes.Remove(existing);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePropertyAttributeAsync(PropertyAttribute existing)
        {
            _context.PropertyAttributes.Update(existing);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Attribute>> GetAttributesListAsync()
        {
            return await _context.Attributes
                .Include(x => x.AttributeDataType)
                .Include(x => x.PossibleValues)
                .ToListAsync();
        }

        public async Task<PropertyAttribute?> FindPropertyAttributeByIdAsync(Guid? propertyAttributeId)
        {
            return await _context.PropertyAttributes
                //.Include(x=>x.Attribute)
                //.ThenInclude(x=>x.AttributeDataType)
                //.Include(x => x.Attribute)
                //.ThenInclude(x=> x.PossibleValues)
                .Where(x => x.Id == propertyAttributeId)
                .FirstOrDefaultAsync();
        }

        public async Task<Attribute?> FindAttributeByIdAsync(Guid? attributeId)
        {
            return await _context.Attributes
                .Where(x => x.Id == attributeId)
                .FirstOrDefaultAsync();
        }

        public Task<AttributeDataType?> FindAttributeDataTypeByIdAsync(Guid attributeDataTypeId)
        {
            return _context.AttributeDataTypes.Where(x=>x.Id==attributeDataTypeId).FirstOrDefaultAsync();
        }

        public async Task<AttributePossibleValue?> FindAttributePossibleValueByIdAsync(Guid? possibleValueId)
        {
            return await _context.AttributePossibleValues.Where(x => x.Id == possibleValueId).FirstOrDefaultAsync();
        }
    }
}
