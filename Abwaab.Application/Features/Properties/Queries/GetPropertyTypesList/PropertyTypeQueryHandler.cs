using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Features.Properties.Common.DTOs;
using Abwaab.Domain.Entities.PropertyEntities;
using MediatR;

namespace Abwaab.Application.Features.Properties.Queries.GetPropertyTypesList
{
    public class PropertyTypeQueryHandler : IRequestHandler<PropertyTypeQuery, List<PropertyTypeResponse>>
    {
        private readonly IPropertyTypeService _propertyTypeService;

        public PropertyTypeQueryHandler(IPropertyTypeService propertyTypeService)
        {
            _propertyTypeService = propertyTypeService;
        }

        public async Task<List<PropertyTypeResponse>> Handle(PropertyTypeQuery request, CancellationToken cancellationToken)
        {
            List<PropertyTypeDTO> propertyTypes = await _propertyTypeService.GetProperyTypesListAsync();
            List<PropertyTypeResponse> response = new();
            foreach (var propertyType in propertyTypes)
                response.Add(new() { Id = propertyType.TypeId, Name = propertyType.TypeName });
            return response;
        }
    }
}
