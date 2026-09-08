using Abwaab.Application.Common.Constants;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Features.Properties.Common.DTOs;
using Abwaab.Domain.Entities.PropertyEntities;
using MediatR;

namespace Abwaab.Application.Features.Visitors.SearchForm;

public class SearchFormQueryHandler : IRequestHandler<SearchFormQuery, SearchFormResponse>
{
    private readonly IPropertyService _propertyService;
    private readonly IPropertyFinishingService _propertyFinishingService;
    private readonly IPropertyTypeService _propertyTypeService;
    private readonly IPropertyAttributeService _propertyAttributeService;
    private readonly IPropertyStatesService _propertyStatesService;

    private readonly string errorTitle = ErrorTitle.SearchForm;

    public SearchFormQueryHandler(IPropertyService propertyService, IPropertyFinishingService propertyFinishingService, IPropertyTypeService propertyTypeService, IPropertyAttributeService propertyAttributeService, IPropertyStatesService propertyStatesService)
    {
        _propertyService = propertyService;
        _propertyFinishingService = propertyFinishingService;
        _propertyTypeService = propertyTypeService;
        _propertyAttributeService = propertyAttributeService;
        _propertyStatesService = propertyStatesService;
    }

    public async Task<SearchFormResponse> Handle(SearchFormQuery request, CancellationToken cancellationToken)
    {
        PropertyState propertyState = await _propertyStatesService.GetPublishedPropertyStateAsync(errorTitle);

        decimal maxPrice = await _propertyService.GetMaxPriceAsync(propertyState);
        decimal minPrice = await _propertyService.GetMinPriceAsync(propertyState);
        decimal maxArea = await _propertyService.GetMaxAreaAsync(propertyState);
        decimal minArea = await _propertyService.GetMinAreaAsync(propertyState);
             
        List<PropertyFinishingDTO> propertyFinishings = await _propertyFinishingService.GetPropertyFinishingListAsync();
        List<PropertyTypeDTO> propertyTypes = await _propertyTypeService.GetProperyTypesListAsync();
        List<PropertyViewSideDTO> propertyViewSides = await _propertyAttributeService.GetPropertyViewSidesListAsync();

        return new SearchFormResponse
        {
            MaxPrice = maxPrice,
            MinPrice = minPrice,
            MaxArea = maxArea,
            MinArea = minArea,
            PropertyFinishings = propertyFinishings,
            PropertyTypes = propertyTypes,
            PropertyViewSides = propertyViewSides
        };
    }
}