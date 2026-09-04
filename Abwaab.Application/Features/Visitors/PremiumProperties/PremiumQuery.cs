using MediatR;

namespace Abwaab.Application.Features.Visitors.PremiumProperties;

public class PremiumQuery: IRequest<PremiumResponse>
{
    public int PageNo { get; set; }
}
