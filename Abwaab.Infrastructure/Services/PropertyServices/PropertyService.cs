using Abwaab.Application.Common.Exceptions.Properties.Attributes;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Features.Properties.Queries.UserProperties;
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

        public async Task<Property> FindPropertyByIdForUpdateAsync(Guid propertyId, string errorTitle)
        {
            Property? property = await _propertyRepository.FindPropertyByIdForUpdateAsync(propertyId);

            if (property == null)
                throw new PropertyNotFoundException(errorTitle);

            return property;
        }

        public async Task<int> GetStaredPropertyCountInPlanAsync(Guid userPlandId)
        {
            return await _propertyRepository.GetStaredPropertyCountInPlanAsync(userPlandId);
        }

        public async Task<List<UserPropertiesResponse>> GetUserPropertiesSummaryAsync(Guid userId)
        {
            List<Property> propertyList = await _propertyRepository.GetUserPropertiesList(userId);

            List<UserPropertiesResponse> response = new();
            foreach (var property in propertyList)
                response.Add(new()
                {
                    propertyId = property.Id,
                    AreaInSquareMeter = property.AreaInSquareMeter,
                    CoverImage = property.MediaList?.FirstOrDefault()?.FilePath,
                    Price = property.Price,
                    PropertyFinishing = property.Finishing?.FinishingName,
                    PropertyType = property.PropertyType?.TypeName,
                    PropertyState = property.PropertyState.StateName,
                    Title = property.Title!
                });

            return response;
        }
    }
}
