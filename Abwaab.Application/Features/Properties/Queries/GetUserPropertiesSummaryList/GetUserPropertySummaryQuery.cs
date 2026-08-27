using MediatR;

namespace Abwaab.Application.Features.Properties.Queries.GetUserPropertiesSummaryList
{
    public class GetUserPropertySummaryQuery : IRequest<List<GetUserPropertySummaryResponse>>
    {
    }
}
