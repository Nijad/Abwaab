using Abwaab.Application.Common.Enums;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Contracts;
using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Domain.Entities.UserEntities;

namespace Abwaab.Infrastructure.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;

        public PropertyService(
            IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        public async Task<Guid> CreatePropertyAsync(UserPlan userPlan, string errorTitle)
        {
            Guid id = Guid.NewGuid();
            PropertyState preparingPropertyState = await GetPreparingPropertyStateAsync(errorTitle);
            Property property = new()
            {
                Id = id,
                UserPlan = userPlan,
                PropertyState = preparingPropertyState
            };

            await _propertyRepository.CreateProperty(property);
            return id;
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

        public async Task<bool> HasBalanceToAddPropertyAsync(UserPlan userPlan)
        {
            int propertyCount = await _propertyRepository.GetPropertiesCountBelongToPlanAsync(userPlan.Id);

            return propertyCount < userPlan.Plan.MaxPropertiesCountAtSameTime;
        }
    }
}
