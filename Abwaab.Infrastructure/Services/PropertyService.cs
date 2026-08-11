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

        public async Task<Guid> CreatePropertyAsync(UserPlan userPlan)
        {
            Guid id = Guid.NewGuid();
            PropertyState preparingPropertyState = await GetPreparingPropertyStateAsync();
            Property property = new()
            {
                Id = id,
                UserPlan = userPlan,
                PropertyState = preparingPropertyState
            };

            await _propertyRepository.CreateProperty(property);
            return id;
        }

        public async Task<PropertyState> GetPreparingPropertyStateAsync() => await FindPropertyStateByStateNameAsync(PropertyStatesEnum.Preparing.ToString());

        public async Task<PropertyState> GetPendingPropertyStateAsync() => await FindPropertyStateByStateNameAsync(PropertyStatesEnum.Pending.ToString());

        public async Task<PropertyState> GetPublishedPropertyStateAsync() => await FindPropertyStateByStateNameAsync(PropertyStatesEnum.Published.ToString());

        public async Task<PropertyState> GetRejectedPropertyStateAsync() => await FindPropertyStateByStateNameAsync(PropertyStatesEnum.Rejected.ToString());

        public async Task<PropertyState> GetSoldPropertyStateAsync() => await FindPropertyStateByStateNameAsync(PropertyStatesEnum.Sold.ToString());

        public async Task<PropertyState> GetDisabledPropertyStateAsync() => await FindPropertyStateByStateNameAsync(PropertyStatesEnum.Disabled.ToString());

        public async Task<PropertyState> GetDeletedPropertyStateAsync() => await FindPropertyStateByStateNameAsync(PropertyStatesEnum.Deleted.ToString());

        public async Task<PropertyState> FindPropertyStateByStateNameAsync(string propertyStateName)
        {
            PropertyState? propertyState = await _propertyRepository.FindPropertyStateByStateNameAsync(propertyStateName);

            if (propertyState == null)
                throw new NotFoundException(nameof(PropertyState), nameof(propertyStateName), propertyStateName);

            return propertyState;
        }

        public async Task<bool> HasBalanceToAddPropertyAsync(UserPlan userPlan)
        {
            int propertyCount = await _propertyRepository.GetPropertiesCountBelongToPlanAsync(userPlan.Id);

            return propertyCount < userPlan.Plan.MaxPropertiesCountAtSameTime;
        }
    }
}
