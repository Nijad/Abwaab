using Abwaab.Application.Common.Enums;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.PropertyEntities;

namespace Abwaab.Infrastructure.Services.PropertyServices
{
    public class PropertyStatesService : IPropertyStatesService
    {
        private readonly IPropertyRepository _propertyRepository;

        public PropertyStatesService(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        public async Task<PropertyState> GetPreparingPropertyStateAsync(string errorTitle) => await FindPropertyStateByStateNameAsync(PropertyStatesEnum.Preparing.ToString(), errorTitle);

        public async Task<PropertyState> GetPendingPropertyStateAsync(string errorTitle) => await FindPropertyStateByStateNameAsync(PropertyStatesEnum.Pending.ToString(), errorTitle);

        public async Task<PropertyState> GetPublishedPropertyStateAsync(string errorTitle) => await FindPropertyStateByStateNameAsync(PropertyStatesEnum.Published.ToString(), errorTitle);

        public async Task<PropertyState> GetRejectedPropertyStateAsync(string errorTitle) => await FindPropertyStateByStateNameAsync(PropertyStatesEnum.Rejected.ToString(), errorTitle);

        public async Task<PropertyState> GetSoldPropertyStateAsync(string errorTitle) => await FindPropertyStateByStateNameAsync(PropertyStatesEnum.Sold.ToString(), errorTitle);

        public async Task<PropertyState> GetDisabledPropertyStateAsync(string errorTitle) => await FindPropertyStateByStateNameAsync(PropertyStatesEnum.Disabled.ToString(), errorTitle);

        public async Task<PropertyState> GetDeletedPropertyStateAsync(string errorTitle) => await FindPropertyStateByStateNameAsync(PropertyStatesEnum.Deleted.ToString(), errorTitle);

        public async Task<PropertyState> FindPropertyStateByStateNameAsync(string propertyStateName, string errorTitle)
        {
            PropertyState? propertyState = await _propertyRepository.FindPropertyStateByStateNameAsync(propertyStateName);

            if (propertyState == null)
                throw new NotFoundException(nameof(PropertyState), nameof(propertyStateName), propertyStateName, errorTitle);

            return propertyState;
        }

        public async Task<PropertyState> GetNewStateForUpdate(PropertyState propertyState, string errorTitle)
        {
            PropertyState rejected = await GetRejectedPropertyStateAsync(errorTitle);
            PropertyState pending = await GetPendingPropertyStateAsync(errorTitle);

            if (propertyState.Id == rejected.Id || propertyState.Id == pending.Id)
                return await GetPreparingPropertyStateAsync(errorTitle);

            return propertyState;
        }
    }
}
