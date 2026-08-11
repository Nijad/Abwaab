
using Abwaab.Domain.Entities.PropertyEntities;

namespace Abwaab.Application.Repositories
{
    public interface IPropertyRepository
    {
        Task CreateProperty(Property property);
        Task<PropertyState?> FindPropertyStateByStateNameAsync(string propertyStateName);
        Task<int> GetPropertiesCountBelongToPlanAsync(Guid planId);
    }
}
