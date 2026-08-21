using Abwaab.Domain.Entities.PropertyEntities;
using Attribute = Abwaab.Domain.Entities.PropertyEntities.Attribute;

namespace Abwaab.Application.Repositories
{
    public interface IAttributeRepository
    {
        Task AddPropertyAttribute(PropertyAttribute comming);
        Task DeletePropertyAttributeAsync(PropertyAttribute existing);
        Task UpdatePropertyAttributeAsync(PropertyAttribute existing);
        Task<List<Attribute>> GetAttributesListAsync();
    }
}
