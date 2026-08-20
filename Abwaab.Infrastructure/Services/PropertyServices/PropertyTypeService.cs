using Abwaab.Application.Common.Exceptions.Properties;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Features.Properties.Common;
using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.PropertyEntities;
using System.ComponentModel;

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

        public async Task<List<PropertyTypeDTO>> GetProperyTypesListAsync()
        {
            var typesList = await _propertyRepository.GetProperyTypesList();
            List<PropertyTypeDTO> ptl = new();
            foreach (PropertyType type in typesList)
                ptl.Add(new() { TypeId = type.Id, TypeName = type.TypeName });
            return ptl;
        }
    }
}
