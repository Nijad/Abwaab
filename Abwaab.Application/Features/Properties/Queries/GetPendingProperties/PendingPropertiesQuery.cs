using MediatR;

namespace Abwaab.Application.Features.Properties.Queries.GetPendingProperties;

public class PendingPropertiesQuery : IRequest<List<PendingPropertiesResponse>>
{
}