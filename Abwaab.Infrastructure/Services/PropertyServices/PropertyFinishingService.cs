using Abwaab.Application.Common.Exceptions.Properties;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Features.Properties.Queries.GetFinishingList;
using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.PropertyEntities;

namespace Abwaab.Infrastructure.Services.PropertyServices
{
    public class PropertyFinishingService : IPropertyFinishingService
    {
        private readonly IPropertyRepository _propertyRepository;

        public PropertyFinishingService(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        public async Task<Finishing> FindPropertyFinishingByIdAsycn(Guid finishingId, string errorTitle)
        {
            Finishing? finishing = await _propertyRepository.FindPropertyFinishingByIdAsync(finishingId);

            if (finishing == null)
                throw new PropertyFinishingNotFoundException(errorTitle);

            return finishing;
        }

        public async Task<List<FinishingRespons>> GetPropertyFinishingListAsync()
        {
            List<Finishing> finishings = await _propertyRepository.GetPropertyFinishingListAsync();
            List<FinishingRespons> finishingRespons = new();
            foreach (var item in finishings)
                finishingRespons.Add(new() { Id = item.Id, Name = item.FinishingName });
            return finishingRespons;
        }
    }
}
