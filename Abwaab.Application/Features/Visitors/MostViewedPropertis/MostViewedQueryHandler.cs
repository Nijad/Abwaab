using Abwaab.Application.Common.Constants;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Features.Visitors.DTOs.MainPage;
using Abwaab.Domain.Entities.PropertyEntities;
using MediatR;
using Attribute = Abwaab.Domain.Entities.PropertyEntities.Attribute;

namespace Abwaab.Application.Features.Visitors.MostViewedPropertis;

public class MostViewedQueryHandler : IRequestHandler<MostViewedQuery, MostViewedResponse>
{
    private readonly IPropertyService _propertyService;
    private readonly IPropertyStatesService _propertyStatesService;
    private readonly IPropertyAttributeService _propertyAttributeService;
    private readonly string errorTitle = ErrorTitle.MainPage;

    public MostViewedQueryHandler(
        IPropertyService propertyService,
        IPropertyStatesService propertyStatesService,
        IPropertyAttributeService propertyAttributeService)
    {
        _propertyService = propertyService;
        _propertyStatesService = propertyStatesService;
        _propertyAttributeService = propertyAttributeService;
    }

    public async Task<MostViewedResponse> Handle(MostViewedQuery request, CancellationToken cancellationToken)
    {
        int take = GeneralConstants.PAGE_COUNT;
        int skip = (request.PageNo - 1) * take;

        PropertyState publishedProperties = await _propertyStatesService.GetPublishedPropertyStateAsync(errorTitle);

        int totalPropertiesCount = await _propertyService.GetTotalPropertiesCountAsync(publishedProperties);
        int pagesCount = (int)Math.Ceiling((double)totalPropertiesCount / take);

        List<Attribute> viewSides = await _propertyAttributeService.GetViewSidesAsync();

        List<MostViewed> mostViewedList = await _propertyService.GetMostViewedPropertiesAsync(publishedProperties, viewSides, skip, take);

        return new MostViewedResponse() { PagesCount = pagesCount, Properties = mostViewedList.Skip(skip).Take(take).ToList() };
    }
}