using Abwaab.Application.Common.Constants;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Features.Visitors.DTOs;
using Abwaab.Application.Features.Visitors.MostViewedPropertis;
using Abwaab.Domain.Entities.PropertyEntities;
using MediatR;
using Attribute = Abwaab.Domain.Entities.PropertyEntities.Attribute;

namespace Abwaab.Application.Features.Visitors.RecentlyAddedProperties;

public class RecentlyAddedQueryHandler : IRequestHandler<RecentlyAddedQuery, RecentlyAddedResponse>
{
    private readonly IPropertyService _propertyService;
    private readonly IPropertyStatesService _propertyStatesService;
    private readonly IPropertyAttributeService _propertyAttributeService;
    private readonly string errorTitle = ErrorTitle.MainPage;

    public RecentlyAddedQueryHandler(
        IPropertyService propertyService, 
        IPropertyStatesService propertyStatesService, 
        IPropertyAttributeService propertyAttributeService)
    {
        _propertyService = propertyService;
        _propertyStatesService = propertyStatesService;
        _propertyAttributeService = propertyAttributeService;
    }

    public async Task<RecentlyAddedResponse> Handle(RecentlyAddedQuery request, CancellationToken cancellationToken)
    {
        int take = GeneralConstants.PAGE_COUNT;
        int skip = (request.PageNo - 1) * take;

        PropertyState publishedProperties = await _propertyStatesService.GetPublishedPropertyStateAsync(errorTitle);

        int totalPropertiesCount = await _propertyService.GetTotalPropertiesCountAsync(publishedProperties);
        int pagesCount = (int)Math.Ceiling((double)totalPropertiesCount / take);

        List<Attribute> viewSides = await _propertyAttributeService.GetViewSidesAsync();


        List<RecentlyAdded> recentlyAddedList = await _propertyService.GetRecentlyAddedPropertiesAsync(publishedProperties, viewSides, skip, take);

        return new RecentlyAddedResponse() { PagesCount = pagesCount, RecentlyAddedList = recentlyAddedList };
    }
}