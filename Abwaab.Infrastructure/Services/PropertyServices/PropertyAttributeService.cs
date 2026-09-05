using Abwaab.Application.Common.Exceptions.Properties.Attributes;
using Abwaab.Application.Common.Exceptions.Properties.DataTypes;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Features.Properties.Common.DTOs;
using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Infrastructure.Services.Common;
using Attribute = Abwaab.Domain.Entities.PropertyEntities.Attribute;

namespace Abwaab.Infrastructure.Services.PropertyServices;

public class PropertyAttributeService : IPropertyAttributeService
{
    private readonly IAttributeRepository _attributeRepository;

    public PropertyAttributeService(IAttributeRepository attributeRepository)
    {
        _attributeRepository = attributeRepository;
    }

    public async Task<List<AttributeDTO>> GetAttributesListAsync()
    {
        List<Attribute> attributes = await _attributeRepository.GetAttributesListAsync();

        List<AttributeDTO> list = new();
        foreach (var attribute in attributes)
        {
            List<AttributePossibleValueDTO> apvl = null!;
            if (attribute.PossibleValues != null)
            {
                apvl = new();
                foreach (var possibleValue in attribute.PossibleValues)
                    apvl.Add(new()
                    {
                        PossibleValueId = possibleValue.Id,
                        PossibleValueDescription = possibleValue.Value
                    });
            }

            list.Add(new()
            {
                AttributeId = attribute.Id,
                AttributeName = attribute.AttributeName,
                DataTypeId = attribute.AttributeDataTypeId,
                DatayTypeDescription = attribute.AttributeDataType!.Name,
                PossibleValues = apvl
            });
        }

        return list;
    }

    public async Task SyncronizePropertyAttributesAsync(
        List<PropertyAttribute>? existingPropertyAttributes,
        List<PropertyAttributeDTO>? commingPropertyAttributes,
        Guid propertyId)
    {
        await SyncronizingCollection.Sync(
            existingPropertyAttributes, commingPropertyAttributes,
            (existing, comming) => existing.Id == comming.PropertyAttributeId,
            async (existing) => await DeletePropertyAttributeAsync(existing),
            async (existing, comming) => await UpdatePropertyAttributeAsync(existing, comming),
            async (comming) => await AddPropertyAttributeAsync(comming, propertyId));
    }

    private async Task AddPropertyAttributeAsync(PropertyAttributeDTO comming, Guid propertyId)
    {
        PropertyAttribute propertyAttribute = new()
        {
            Id = Guid.NewGuid(),
            PropertyId = propertyId,
            AttributeId = (Guid)comming.AttributeId!,
            AttributeValue = comming.Value!
        };

        await _attributeRepository.AddPropertyAttribute(propertyAttribute);
    }

    private async Task UpdatePropertyAttributeAsync(PropertyAttribute existing, PropertyAttributeDTO comming)
    {
        existing.AttributeId = (Guid)comming.AttributeId!;
        existing.AttributeValue = comming.Value!;
        await _attributeRepository.UpdatePropertyAttributeAsync(existing);
    }

    private async Task DeletePropertyAttributeAsync(PropertyAttribute existing)
    {
        await _attributeRepository.DeletePropertyAttributeAsync(existing);
    }

    public async Task<PropertyAttribute> FindPropertyAttributeByIdAsync(Guid? propertyAttributeId, string errorTitle)
    {
        PropertyAttribute? propertyAttribute = await _attributeRepository.FindPropertyAttributeByIdAsync(propertyAttributeId);
        if (propertyAttribute == null)
            throw new PropertyAttributeNotFoundException(errorTitle);
        return propertyAttribute;
    }

    public async Task<Attribute> FindAttributeByIdAsync(Guid? attributeId, string errorTitle)
    {
        Attribute? attribute = await _attributeRepository.FindAttributeByIdAsync(attributeId);
        if (attribute == null)
            throw new AttributeNotFoundException(errorTitle);
        return attribute;
    }

    public async Task<AttributeDataType> FindAttributeDataTypeByIdAsync(Guid attributeDataTypeId, string errorTitle)
    {
        AttributeDataType? attributeDataType = await _attributeRepository.FindAttributeDataTypeByIdAsync(attributeDataTypeId);
        if (attributeDataType == null)
            throw new DataTypeNotImplementedException(errorTitle);
        return attributeDataType;
    }

    public async Task<AttributePossibleValue> FindAttributePossibleValueByIdAsync(Guid? possibleValueId, string errorTitle)
    {
        AttributePossibleValue? attributePossibleValue = await _attributeRepository.FindAttributePossibleValueByIdAsync(possibleValueId);

        if (attributePossibleValue == null)
            throw new AttributePossibleValueNotFoundException(errorTitle);

        return attributePossibleValue;
    }

    public async Task<List<Attribute>> GetViewSidesAsync()
    {
        List<Attribute> attributes = await _attributeRepository.GetAttributesListAsync();
        List<Attribute> sides = attributes.Where(x => x.AttributeName == "شمالي" || x.AttributeName == "جنوبي" || x.AttributeName == "شرقي" || x.AttributeName == "غربي").ToList();
        return sides;
    }

    public async Task<List<PropertyViewSideDTO>> GetPropertyViewSidesListAsync()
    {
        var sides = await GetViewSidesAsync();
        var response = sides.Select(s => new PropertyViewSideDTO()
        {
            AttributeId = s.Id,
            AttributeName = s.AttributeName
        }).ToList();
        return response;
    }
}
