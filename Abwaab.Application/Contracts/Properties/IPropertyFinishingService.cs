using Abwaab.Application.Features.Properties.Common.DTOs;
using Abwaab.Domain.Entities.PropertyEntities;

namespace Abwaab.Application.Contracts.Properties
{
    public interface IPropertyFinishingService
    {
        Task<Finishing> FindPropertyFinishingByIdAsycn(Guid finishingId, string errorTitle);
        Task<List<PropertyFinishingDTO>> GetPropertyFinishingListAsync();
    }
}
