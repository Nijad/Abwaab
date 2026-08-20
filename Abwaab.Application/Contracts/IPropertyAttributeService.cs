using Abwaab.Application.Features.Properties.Common;

namespace Abwaab.Application.Contracts
{
    public interface IPropertyAttributeService
    {
        Task<List<AttributeDTO>> GetAttributesListAsync();
    }
}
