using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Features.Properties.Common.DTOs;
using MediatR;

namespace Abwaab.Application.Features.Visitors.SearchForm;

public class SearchFormQueryHandler : IRequestHandler<SearchFormQuery, SearchFormResponse>
{
    private readonly IPropertyService _propertyService;
    private readonly IPropertyFinishingService _propertyFinishingService;
    private readonly IPropertyTypeService _propertyTypeService;
    private readonly IPropertyAttributeService _propertyAttributeService;

    public SearchFormQueryHandler(IPropertyService propertyService, IPropertyFinishingService propertyFinishingService, IPropertyTypeService propertyTypeService, IPropertyAttributeService propertyAttributeService)
    {
        _propertyService = propertyService;
        _propertyFinishingService = propertyFinishingService;
        _propertyTypeService = propertyTypeService;
        _propertyAttributeService = propertyAttributeService;
    }

    public async Task<SearchFormResponse> Handle(SearchFormQuery request, CancellationToken cancellationToken)
    {
        decimal maxPrice = await _propertyService.GetMaxPriceAsync();
        decimal minPrice = await _propertyService.GetMinPriceAsync();
        decimal maxArea = await _propertyService.GetMaxAreaAsync();
        decimal minArea = await _propertyService.GetMinAreaAsync();

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