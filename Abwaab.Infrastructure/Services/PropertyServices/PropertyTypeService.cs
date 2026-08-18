using Abwaab.Application.Common.Exceptions.Properties;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Features.Properties.Queries.GetPropertyTypesList;
using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.PropertyEntities;

namespace Abwaab.Infrastructure.Services.PropertyServices
{
    public class PropertyTypeService : IPropertyTypeService
    {
        private readonly IPropertyRepository _propertyRepository;

        public PropertyTypeService(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        public async Task<PropertyType> FindPropertyTypeByIdAsync(Guid propertyTypeId, string errorTitle)
        {
            PropertyType? propertyType = await _propertyRepository.FindPropertyTypeByIdAsync(propertyTypeId);

            if(propertyType == null)
                throw new PropertyTypeNotFoundException(errorTitle);

            return propertyType;
        }

        public async Task<List<PropertyTypeResponse>> GetProperyTypesList()
        {
            List<PropertyType> propertyTypes = await _propertyRepository.GetProperyTypesList();
            List<PropertyTypeResponse> response = new();
            foreach (PropertyType propertyType in propertyTypes)
                response.Add(new() { Id = propertyType.Id, Name = propertyType.TypeName });
            return response;
        }
    }
}
