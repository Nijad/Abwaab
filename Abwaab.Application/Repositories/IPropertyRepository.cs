using Abwaab.Application.Features.Visitors.Search;
using Abwaab.Domain.Entities.PropertyEntities;
using Attribute = Abwaab.Domain.Entities.PropertyEntities.Attribute;

namespace Abwaab.Application.Repositories;

public interface IPropertyRepository
{
    Task CreateProperty(Property property);
    Task<Property?> FindPropertyByIdAsync(Guid propertyId);
    Task<Property?> FindPropertyByIdForUpdateAsync(Guid propertyId);
    Task<Finishing?> FindPropertyFinishingByIdAsync(Guid finishingId);
    Task<PropertyState?> FindPropertyStateByStateNameAsync(string propertyStateName);
    Task<PropertyType?> FindPropertyTypeByIdAsync(Guid propertyTypeId);
    Task<Property?> FindPropertyWithUserAndStateByIdAsync(Guid propertyId);
    Task<decimal> GetMaxAreaAsync();
    Task<decimal> GetMaxPriceAsync();
    Task<decimal> GetMinAreaAsync();
    Task<decimal> GetMinPriceAsync();
    Task<List<Property>> GetMostViewedPropertiesAsync(PropertyState publishedProperties, int skip, int take);
    Task<List<Property>> GetPremiumPropertiesAsync(PropertyState publishedProperties, int skip, int take);
    Task<List<Property>> GetPropertiesByStateAsync(PropertyState pendingProperties);
    Task<int> GetPropertiesCountBelongToPlanAsync(Guid planId);
    Task<List<Finishing>> GetPropertyFinishingListAsync();
    Task<List<PropertyType>> GetProperyTypesList();
    Task<List<Property>> GetRecentlyAddedPropertiesAsync(PropertyState publishedProperties, int skip, int take);
    Task<int> GetStaredPropertyCountInPlanAsync(Guid userPlandId);
    Task<int> GetTotalPremiumPropertiesCountAsync(PropertyState publishedProperties);
    Task<int> GetTotalPropertiesCountAsync(PropertyState publishedProperties);
    Task<List<Property>> GetUserPropertiesList(Guid userId);
    Task<bool> PropertyBelongToUser(Guid userId, Guid propertyId);
    Task<List<Property>> SearchPropertiesAsync(SearchQuery request, List<Attribute> viewSides);
    Task UpdatePropertyAsync(Property property);
}
