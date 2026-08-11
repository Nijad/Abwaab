
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Domain.Entities.UserEntities;

namespace Abwaab.Application.Contracts
{
    public interface IPropertyService
    {
        Task<Guid> CreatePropertyAsync(UserPlan userPlan);
        Task<bool> HasBalanceToAddPropertyAsync(UserPlan userPlan);
        Task<PropertyState> GetPreparingPropertyStateAsync();
        Task<PropertyState> GetPendingPropertyStateAsync();
        Task<PropertyState> GetPublishedPropertyStateAsync();
        Task<PropertyState> GetRejectedPropertyStateAsync();
        Task<PropertyState> GetSoldPropertyStateAsync();
        Task<PropertyState> GetDisabledPropertyStateAsync();
        Task<PropertyState> GetDeletedPropertyStateAsync();
        Task<PropertyState> FindPropertyStateByStateNameAsync(string propertyStateName);


    }
}
