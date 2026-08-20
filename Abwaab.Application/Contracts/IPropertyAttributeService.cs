using Abwaab.Application.Features.Properties.Queries.GetPropertyForUpdate;

namespace Abwaab.Application.Contracts
{
    public interface IPropertyAttributeService
    {
        Task<List<AttributeForUpdate>> GetAttributesListAsync();
    }
}
