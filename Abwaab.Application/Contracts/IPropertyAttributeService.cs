using Abwaab.Application.Features.Properties.Common.DTOs;
using Abwaab.Domain.Entities.PropertyEntities;

namespace Abwaab.Application.Contracts
{
    public interface IPropertyAttributeService
    {
        Task<List<AttributeDTO>> GetAttributesListAsync();
        Task SyncronizePropertyAttributesAsync(
            List<PropertyAttribute>? existingPropertyAttributes,
            List<PropertyAttributeDTO>? commingPropertyAttributes,
            Guid propertyId);
    }
}
