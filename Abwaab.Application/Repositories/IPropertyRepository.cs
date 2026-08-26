using Abwaab.Domain.Entities.PropertyEntities;
using Attribute = Abwaab.Domain.Entities.PropertyEntities.Attribute;

namespace Abwaab.Application.Repositories
{
    public interface IPropertyRepository
    {
        Task CreateProperty(Property property);
        Task<Property?> FindPropertyByIdAsync(Guid propertyId);
        Task<Property?> FindPropertyByIdForUpdateAsync(Guid propertyId);
        Task<Finishing?> FindPropertyFinishingByIdAsync(Guid finishingId);
        Task<PropertyState?> FindPropertyStateByStateNameAsync(string propertyStateName);
        Task<PropertyType?> FindPropertyTypeByIdAsync(Guid propertyTypeId);
        Task<int> GetPropertiesCountBelongToPlanAsync(Guid planId);
        Task<List<Finishing>> GetPropertyFinishingListAsync();
        Task<List<PropertyType>> GetProperyTypesList();
        Task<int> GetStaredPropertyCountInPlanAsync(Guid userPlandId);
        Task<List<Property>> GetUserPropertiesList(Guid userId);
        Task<bool> PropertyBelongToUser(Guid userId, Guid propertyId);
        Task UpdatePropertyAsync(Property property);
    }
}
