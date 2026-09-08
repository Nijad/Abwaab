using Abwaab.Application.Common.Constants;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Domain.Entities.PropertyEntities;
using MediatR;
using Attribute = Abwaab.Domain.Entities.PropertyEntities.Attribute;

namespace Abwaab.Application.Features.Visitors.Search;

public class SearchQueryHandler : IRequestHandler<SearchQuery, List<SearchResponse>>
{
    private readonly IPropertyService _propertyService;
    private readonly IPropertyAttributeService _propertyAttributeService;
    private readonly IPropertyStatesService _propertyStatesService;
    readonly string errorTitle = ErrorTitle.Search;

    public SearchQueryHandler(IPropertyService propertyService, IPropertyAttributeService propertyAttributeService, IPropertyStatesService propertyStatesService)
    {
        _propertyService = propertyService;
        _propertyAttributeService = propertyAttributeService;
        _propertyStatesService = propertyStatesService;
    }

    public async Task<List<SearchResponse>> Handle(SearchQuery request, CancellationToken cancellationToken)
    {
        PropertyState propertyState = await _propertyStatesService.GetPublishedPropertyStateAsync(errorTitle);

        List<Attribute> viewSides = await _propertyAttributeService.GetViewSidesAsync();

        List<SearchResponse> response = await _propertyService.SearchPropertiesAsync(request, viewSides, propertyState);

        return response;
    }
}