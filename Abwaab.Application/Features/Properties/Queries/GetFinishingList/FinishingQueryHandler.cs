using Abwaab.Application.Contracts.Properties;
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
            return await _propertyFinishingService.GetPropertyFinishingListAsync();
        }
    }
}
