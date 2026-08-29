using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Mappings;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Features.Properties.Common.DTOs;
using Abwaab.Domain.Entities.PropertyEntities;
using MediatR;

namespace Abwaab.Application.Features.Properties.Queries.GetPropertyDetails;

public class PropertyDetailsQueryHandler : IRequestHandler<PropertyDetailsQuery, PropertyDetailsResponse>
{
    private readonly IPropertyService _propertyService;
    private readonly string errorTitle = ErrorTitle.PropertyDetails;

    public PropertyDetailsQueryHandler(IPropertyService propertyService)
    {
        _propertyService = propertyService;
    }

    public async Task<PropertyDetailsResponse> Handle(PropertyDetailsQuery request, CancellationToken cancellationToken)
    {
        Property property = await _propertyService.FindPropertyByIdForUpdateAsync(request.PropertyId, errorTitle);

        List<PropertyAttributeBaseDTO> propertyAttributes = null!;
        if (property.PropertyAttributes != null)
        {
            propertyAttributes = new();
            foreach (var propertyAttribute in property.PropertyAttributes)
                propertyAttributes.Add(new()
                {
                    Value = propertyAttribute.AttributeValue,
                    AttributeName = propertyAttribute.Attribute.AttributeName,
                    DataTypeDescription = propertyAttribute.Attribute.AttributeDataType?.Name
                });
        }

        List<MediaBaseDTO> mediaDTOs = new();
        foreach (var media in property.MediaList)
            mediaDTOs.Add(new()
            {
                MediaId = media.Id,
                FilePath = media.FilePath,
                MediaTypeName = media.MediaType.Name,
                IsCover = media.IsCover,
            });

        PropertyDetailsResponse response = new()
        {
            PropertyId = request.PropertyId,
            Title = property.Title,
            Description = property.Description,
            Address = property.Address,
            AreaInSquareMeter = property.AreaInSquareMeter,
            Price = property.Price,
            Latitude = property.Latitude,
            Longitude = property.Longitude,
            PropertyAttributesList = propertyAttributes,
            PropertyMediaList = mediaDTOs,
            PropertyState = PropertySTatesMapping.Map(property.PropertyState.StateName),
            IsStar = property.IsStard,
            PropertyFinishing = property.Finishing?.FinishingName,
            PropertyType = property.PropertyType.TypeName,
            ViewsNumber = property.NumberOfView,
            PublishedAt = property.PublishedAt
        };

        property.NumberOfView++;
        await _propertyService.UpdatePropertyAsync(property);

        return response;
    }
}