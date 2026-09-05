using Abwaab.Application.Common.Constants;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Features.Visitors.DTOs.MainPage;
using Abwaab.Domain.Entities.PropertyEntities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Attribute = Abwaab.Domain.Entities.PropertyEntities.Attribute;

namespace Abwaab.Application.Features.Visitors.MainPage;

public class MainPageQueryHandler : IRequestHandler<MainPageQuery, MainPageResponse>
{
    private readonly IPropertyService _propertyService;
    private readonly IPropertyStatesService _propertyStatesService;
    private readonly IPropertyAttributeService _propertyAttributeService;
    private readonly string errorTitle = ErrorTitle.MainPage;

    public MainPageQueryHandler(IPropertyService propertyService, IPropertyStatesService propertyStatesService, IPropertyAttributeService propertyAttributeService)
    {
        _propertyService = propertyService;
        _propertyStatesService = propertyStatesService;
        _propertyAttributeService = propertyAttributeService;
    }

    public async Task<MainPageResponse> Handle(MainPageQuery request, CancellationToken cancellationToken)
    {
        int skip = 0;
        int take = GeneralConstants.PROPERTIES_COUNT_MAIN_PAGE_LIST;
        PropertyState publishedProperties = await _propertyStatesService.GetPublishedPropertyStateAsync(errorTitle);

        List<Attribute> viewSides = await _propertyAttributeService.GetViewSidesAsync();

        List<RecentlyAdded> recentlyAddedList = await _propertyService.GetRecentlyAddedPropertiesAsync(publishedProperties, viewSides, skip, take);

        List<MostViewed> mostViewedList = await _propertyService.GetMostViewedPropertiesAsync(publishedProperties, viewSides, skip, take);

        int totalRows = await _propertyService.GetTotalPremiumPropertiesCountAsync(publishedProperties);
        Random rand = new Random();
        int maxSkip = Math.Max(0, totalRows - GeneralConstants.PROPERTIES_COUNT_MAIN_PAGE_LIST);
        int randomSkip = rand.Next(0, maxSkip + 1);

        List<Premium> premiumPropertiesList = await _propertyService.GetPremiumPropertiesAsync(publishedProperties, viewSides, randomSkip, take);

        MainPageResponse response = new()
        {
            RecentlyAddedList = recentlyAddedList,
            PremiumPropertiesList = premiumPropertiesList,
            MostViewedList = mostViewedList
        };
        return response;
    }
}
