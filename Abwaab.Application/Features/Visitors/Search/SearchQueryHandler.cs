using Abwaab.Application.Common.Constants;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Features.Visitors.DTOs.MainPage;
using Abwaab.Domain.Entities.PropertyEntities;
using MediatR;
using Attribute = Abwaab.Domain.Entities.PropertyEntities.Attribute;

namespace Abwaab.Application.Features.Visitors.Search;

public class SearchQueryHandler : IRequestHandler<SearchQuery, SearchResponse>
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

    public async Task<SearchResponse> Handle(SearchQuery request, CancellationToken cancellationToken)
    {
        int take = GeneralConstants.PAGE_COUNT;
        int skip = (request.PageNo - 1) * take;

        PropertyState propertyState = await _propertyStatesService.GetPublishedPropertyStateAsync(errorTitle);

        List<Attribute> viewSides = await _propertyAttributeService.GetViewSidesAsync();
        int total = await _propertyService.SearchPropertiesCountAsync(request, viewSides, propertyState);
        int pagesCount = (int)Math.Ceiling((double)total /take);
        List<SearchDTO> response = await _propertyService.SearchPropertiesAsync(request, viewSides, propertyState, skip, take);

        return new SearchResponse()
        {
            PagesCount = pagesCount,
            Properties = response
        };
    }
}