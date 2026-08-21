using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Features.Properties.Common.DTOs;
using Abwaab.Domain.Entities.PropertyEntities;
using MediatR;

namespace Abwaab.Application.Features.Properties.Queries.GetFinishingList
{
    public class FinishingQueryHandler : IRequestHandler<FinishingQuery, List<FinishingRespons>>
    {
        private readonly IPropertyFinishingService _propertyFinishingService;

        public FinishingQueryHandler(IPropertyFinishingService propertyFinishingService)
        {
            _propertyFinishingService = propertyFinishingService;
        }

        public async Task<List<FinishingRespons>> Handle(FinishingQuery request, CancellationToken cancellationToken)
        {
            List<PropertyFinishingDTO> finishings = await _propertyFinishingService.GetPropertyFinishingListAsync();
            
            List<FinishingRespons> finishingRespons = new();
            foreach (var item in finishings)
                finishingRespons.Add(new() { Id = item.FinishingId, Name = item.FinishingName });
            
            return finishingRespons;
        }
    }
}
