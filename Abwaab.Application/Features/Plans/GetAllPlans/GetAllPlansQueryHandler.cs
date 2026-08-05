using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;
namespace Abwaab.Application.Features.Plans.GetAllPlans
{
    public class GetAllPlansQueryHandler : IRequestHandler<GetAllPlansQuery, List<GetAllPlansResponse>>
    {
        private readonly IPlanRepository _planRepository;
        public GetAllPlansQueryHandler(IPlanRepository planRepository)
        {
            _planRepository = planRepository;
        }
        public async Task<List<GetAllPlansResponse>> Handle(GetAllPlansQuery request, CancellationToken cancellationToken)
        {
            List<Plan> plans = await _planRepository.GetAllAsync();
            List<GetAllPlansResponse> response = new();
            foreach (Plan plan in plans)
                response.Add(new() { 
                    Id = plan.Id, 
                    Name = plan.Name,
                    Price = plan.Price,
                    DurationInDays = plan.DurationInDays,
                    StartDate = plan.StartDate,
                    ExpieryDate = plan.ExpieryDate,
                    TempDurationInDays = plan.TempDurationInDays,
                    MaxPropertiesCountAtSameTime = plan.MaxPropertiesCountAtSameTime,
                    MaxStardPropertiesCountAtSameTime = plan.MaxStardPropertiesCountAtSameTime,
                    MaxImagesCount = plan.MaxImagesCount,
                    MaxVideosCount = plan.MaxVideosCount
                });

            return response;
        }
    }
}
