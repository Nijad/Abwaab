using Abwaab.Application.Features.Properties.Common;
using Abwaab.Domain.Entities.PropertyEntities;

namespace Abwaab.Application.Contracts.Properties
{
    public interface IPropertyTypeService
    {
        Task<PropertyType> FindPropertyTypeByIdAsync(Guid propertyTypeId, string errorTitle);
        Task<List<PropertyTypeDTO>> GetProperyTypesListAsync();
    }
}
