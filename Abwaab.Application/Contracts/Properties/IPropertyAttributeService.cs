using Abwaab.Application.Features.Properties.Common.DTOs;
using Abwaab.Domain.Entities.PropertyEntities;
using Attribute = Abwaab.Domain.Entities.PropertyEntities.Attribute;

namespace Abwaab.Application.Contracts.Properties;

public interface IPropertyAttributeService
{
    Task<Domain.Entities.PropertyEntities.Attribute> FindAttributeByIdAsync(Guid? attributeId, string errorTitle);
    Task<AttributeDataType> FindAttributeDataTypeByIdAsync(Guid attributeDataTypeId, string errorTitle);
    Task<AttributePossibleValue> FindAttributePossibleValueByIdAsync(Guid? possibleValueId, string errorTitle);
    Task<PropertyAttribute> FindPropertyAttributeByIdAsync(Guid? propertyAttributeId, string errorTitle);
    Task<List<AttributeDTO>> GetAttributesListAsync();
    Task<List<Attribute>> GetViewSidesAsync();
    Task SyncronizePropertyAttributesAsync(
        List<PropertyAttribute>? existingPropertyAttributes,
        List<PropertyAttributeDTO>? commingPropertyAttributes,
        Guid propertyId);
}
