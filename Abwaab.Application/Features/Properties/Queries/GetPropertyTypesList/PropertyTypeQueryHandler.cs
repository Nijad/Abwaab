using Abwaab.Application.Contracts.Properties;
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
            return await _propertyTypeService.GetProperyTypesList();
        }
    }
}
