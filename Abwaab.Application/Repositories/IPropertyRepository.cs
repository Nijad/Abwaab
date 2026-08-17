
using Abwaab.Domain.Entities.PropertyEntities;

namespace Abwaab.Application.Repositories
{
    public interface IPropertyRepository
    {
        Task CreateProperty(Property property);
        Task<Property?> FindPropertyByIdAsync(Guid propertyId);
        Task<PropertyState?> FindPropertyStateByStateNameAsync(string propertyStateName);
        Task<int> GetPropertiesCountBelongToPlanAsync(Guid planId);
        Task<bool> PropertyBelongToUser(Guid userId, Guid propertyId);
        Task UpdatePropertyAsync(Property property);
    }
}
