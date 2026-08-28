using Abwaab.Application.Features.Properties.Queries.UserProperties;
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Domain.Entities.UserEntities;

namespace Abwaab.Application.Contracts.Properties
{
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
    }
}
