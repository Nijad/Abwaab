using MediatR;

namespace Abwaab.Application.Features.Visitors.MostViewedPropertis;

public class MostViewedQuery : IRequest<MostViewedResponse>
{
    public int PageNo { get; set; }
}
