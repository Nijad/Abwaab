using MediatR;

namespace Abwaab.Application.Features.Visitors.RecentlyAddedProperties;

public class RecentlyAddedQuery : IRequest<RecentlyAddedResponse>
{
    public int PageNo { get; set; }
}
