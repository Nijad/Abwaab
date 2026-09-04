using Abwaab.Application.Contracts.Properties;
using MediatR;
using Attribute = Abwaab.Domain.Entities.PropertyEntities.Attribute;

namespace Abwaab.Application.Features.Visitors.Search;

public class SearchQueryHandler : IRequestHandler<SearchQuery, List<SearchResponse>>
{
    private readonly IPropertyService _propertyService;
    private readonly IPropertyAttributeService _propertyAttributeService;

    public SearchQueryHandler(IPropertyService propertyService, IPropertyAttributeService propertyAttributeService)
    {
        _propertyService = propertyService;
        _propertyAttributeService = propertyAttributeService;
    }

    public async Task<List<SearchResponse>> Handle(SearchQuery request, CancellationToken cancellationToken)
    {
        List<Attribute> viewSides = await _propertyAttributeService.GetViewSidesAsync();

        List<SearchResponse> response = await _propertyService.SearchPropertiesAsync(request, viewSides);

        return response;
    }
}