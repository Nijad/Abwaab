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
            Property property = new()
            {
                Id = id,
                UserPlan = userPlan
            };

            await _propertyRepository.CreateProperty(property);
            return id;
        }

        public async Task<bool> HasBalanceToAddPropertyAsync(UserPlan userPlan)
        {
            int propertyCount = await _propertyRepository.GetPropertiesCountBelongToPlanAsync(userPlan.Id);
            
            return propertyCount < userPlan.Plan.MaxPropertiesCountAtSameTime;
        }
    }
}
