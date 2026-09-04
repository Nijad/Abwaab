using Abwaab.Application.Common.Constants;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Features.Visitors.DTOs;
using Abwaab.Domain.Entities.PropertyEntities;
using MediatR;
using Attribute = Abwaab.Domain.Entities.PropertyEntities.Attribute;

namespace Abwaab.Application.Features.Visitors.PremiumProperties;

public class PremiumQueryHandler : IRequestHandler<PremiumQuery, PremiumResponse>
{
    private readonly IPropertyService _propertyService;
    private readonly IPropertyStatesService _propertyStatesService;
    private readonly IPropertyAttributeService _propertyAttributeService;
    private readonly string errorTitle = ErrorTitle.MainPage;

    public PremiumQueryHandler(
        IPropertyService propertyService,
        IPropertyStatesService propertyStatesService,
        IPropertyAttributeService propertyAttributeService)
    {
        _propertyService = propertyService;
        _propertyStatesService = propertyStatesService;
        _propertyAttributeService = propertyAttributeService;
    }

    public async Task<PremiumResponse> Handle(PremiumQuery request, CancellationToken cancellationToken)
    {

        int take = GeneralConstants.PAGE_COUNT;
        int skip = (request.PageNo - 1) * take;

        PropertyState publishedProperties = await _propertyStatesService.GetPublishedPropertyStateAsync(errorTitle);

        int totalPropertiesCount = await _propertyService.GetTotalPremiumPropertiesCountAsync(publishedProperties);
        int pagesCount = (int)Math.Ceiling((double)totalPropertiesCount / take);

        List<Attribute> viewSides = await _propertyAttributeService.GetViewSidesAsync();

        List<Premium> premiumList = await _propertyService.GetPremiumPropertiesAsync(publishedProperties, viewSides, skip, take);

        return new PremiumResponse() { PagesCount = pagesCount, Properties = premiumList };
    }
}