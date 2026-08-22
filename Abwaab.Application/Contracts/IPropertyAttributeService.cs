using Abwaab.Application.Features.Properties.Common.DTOs;
using Abwaab.Domain.Entities.PropertyEntities;

namespace Abwaab.Application.Contracts
{
    public interface IPropertyAttributeService
    {
        Task<Domain.Entities.PropertyEntities.Attribute> FindAttributeByIdAsync(Guid? attributeId, string errorTitle);
        Task<AttributeDataType> FindAttributeDataTypeByIdAsync(Guid attributeDataTypeId, string errorTitle);
        Task<AttributePossibleValue> FindAttributePossibleValueByIdAsync(Guid? possibleValueId, string errorTitle);
        Task<PropertyAttribute> FindPropertyAttributeByIdAsync(Guid? propertyAttributeId, string errorTitle);
        Task<List<AttributeDTO>> GetAttributesListAsync();
        Task SyncronizePropertyAttributesAsync(
            List<PropertyAttribute>? existingPropertyAttributes,
            List<PropertyAttributeDTO>? commingPropertyAttributes,
            Guid propertyId);
    }
}
