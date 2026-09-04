using Abwaab.Application.Features.Properties.Queries.UserProperties;
using Abwaab.Application.Features.Visitors.DTOs;
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Domain.Entities.UserEntities;
using Attribute = Abwaab.Domain.Entities.PropertyEntities.Attribute;

namespace Abwaab.Application.Contracts.Properties;

public interface IPropertyService
{
    Task<Guid> CreatePropertyAsync(UserPlan userPlan, PropertyState propertyState, string errorTitle);
    Task<bool> HasBalanceToAddPropertyAsync(UserPlan userPlan);
    Task<Property> FindPropertyByIdAsync(Guid propertyId, string errorTitle);
    Task<bool> PropertyBelongToUser(Guid userId, Guid propertyId);
    Task UpdatePropertyAsync(Property property);
    Task<Property> FindPropertyByIdForUpdateAsync(Guid propertyId, string errorTitle);
    Task<int> GetStaredPropertyCountInPlanAsync(Guid userPlandId);
    Task<List<UserPropertiesResponse>> GetUserPropertiesSummaryAsync(Guid userId);
    Task<Property> FindPropertyWithUserAndStateByIdAsync(Guid propertyId, string errorTitle);
    Task<List<RecentlyAdded>> GetRecentlyAddedPropertiesAsync(PropertyState publishedProperties, List<Attribute> viewSides, int skip, int take);
    Task<List<Premium>> GetPremiumPropertiesAsync(PropertyState publishedProperties, List<Attribute> viewSides, int skip, int take);
    Task<List<MostViewed>> GetMostViewedPropertiesAsync(PropertyState publishedProperties, List<Attribute> viewSides, int skip, int take);
    Task<int> GetTotalPropertiesCountAsync(PropertyState publishedProperties);
    Task<int> GetTotalPremiumPropertiesCountAsync(PropertyState publishedProperties);
}
