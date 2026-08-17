
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Domain.Entities.UserEntities;

namespace Abwaab.Application.Contracts
{
    public interface IPropertyService
    {
        Task<Guid> CreatePropertyAsync(UserPlan userPlan, string errorTitle);
        Task<bool> HasBalanceToAddPropertyAsync(UserPlan userPlan);
        Task<PropertyState> GetPreparingPropertyStateAsync(string errorTitle);
        Task<PropertyState> GetPendingPropertyStateAsync(string errorTitle);
        Task<PropertyState> GetPublishedPropertyStateAsync(string errorTitle);
        Task<PropertyState> GetRejectedPropertyStateAsync(string errorTitle);
        Task<PropertyState> GetSoldPropertyStateAsync(string errorTitle);
        Task<PropertyState> GetDisabledPropertyStateAsync(string errorTitle);
        Task<PropertyState> GetDeletedPropertyStateAsync(string errorTitle);
        Task<PropertyState> FindPropertyStateByStateNameAsync(string propertyStateName, string errorTitle);
        Task<Property> FindPropertyByIdAsync(Guid propertyId, string errorTitle);
        Task<bool> PropertyBelongToUser(Guid userId, Guid propertyId);
        Task<PropertyState> GetNewState(PropertyState propertyState, string errorTitle);
        Task UpdatePropertyAsync(Property property);
    }
}
