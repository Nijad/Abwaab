using Abwaab.Domain.Entities.PropertyEntities;

namespace Abwaab.Application.Contracts.Properties
{
    public interface IPropertyStatesService
    {
        Task<PropertyState> GetPreparingPropertyStateAsync(string errorTitle);
        Task<PropertyState> GetPendingPropertyStateAsync(string errorTitle);
        Task<PropertyState> GetPublishedPropertyStateAsync(string errorTitle);
        Task<PropertyState> GetRejectedPropertyStateAsync(string errorTitle);
        Task<PropertyState> GetSoldPropertyStateAsync(string errorTitle);
        Task<PropertyState> GetDisabledPropertyStateAsync(string errorTitle);
        Task<PropertyState> GetDeletedPropertyStateAsync(string errorTitle);
        Task<PropertyState> FindPropertyStateByStateNameAsync(string propertyStateName, string errorTitle);
        Task<PropertyState> GetNewState(PropertyState propertyState, string errorTitle);
    }
}
