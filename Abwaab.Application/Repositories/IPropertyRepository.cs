
using Abwaab.Domain.Entities.PropertyEntities;

namespace Abwaab.Application.Repositories
{
    public interface IPropertyRepository
    {
        Task CreateProperty(Property property);
        Task<int> GetPropertiesCountBelongToPlanAsync(Guid planId);
    }
}
