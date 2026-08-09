using MediatR;
namespace Abwaab.Application.Features.Plans.GetAllPlans
{
    public class GetAllPlansQuery : IRequest<List<GetAllPlansResponse>>
    {
    }
}
