using Abwaab.Application.Common.Enums;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Properties;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Domain.Entities.UserEntities;

namespace Abwaab.Infrastructure.Services.PropertyServices
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;

        public PropertyService(
            IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        public async Task<Guid> CreatePropertyAsync(UserPlan userPlan, PropertyState propertyState, string errorTitle)
        {
            Guid id = Guid.NewGuid();
            
            Property property = new()
            {
                Id = id,
                UserPlan = userPlan,
                PropertyState = propertyState
            };

            await _propertyRepository.CreateProperty(property);
            return id;
        }
        
        public async Task<bool> HasBalanceToAddPropertyAsync(UserPlan userPlan)
        {
            int propertyCount = await _propertyRepository.GetPropertiesCountBelongToPlanAsync(userPlan.Id);

            return propertyCount < userPlan.Plan.MaxPropertiesCountAtSameTime;
        }

        public async Task<Property> FindPropertyByIdAsync(Guid propertyId, string errorTitle)
        {
            Property? property = await _propertyRepository.FindPropertyByIdAsync(propertyId);

            if (property == null)
                throw new PropertyNotFoundException(errorTitle);

            return property;
        }

        public async Task<bool> PropertyBelongToUser(Guid userId, Guid propertyId)
        {
            return await _propertyRepository.PropertyBelongToUser(userId, propertyId);
        }
                
        public async Task UpdatePropertyAsync(Property property)
        {
            await _propertyRepository.UpdatePropertyAsync(property);
        }
    }
}
