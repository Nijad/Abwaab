using Abwaab.Application.Contracts;
using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;

namespace Abwaab.Application.Features.Plans.CreatePlan
{
    public class CreatePlanCommandHandler : IRequestHandler<CreatePlanCommand, CreatePlanResponse>
    {
        private readonly IPlanRepository _planRepository;
        private readonly IUserService _userService;
        public CreatePlanCommandHandler(IPlanRepository planRepository, IUserService userService)
        {
            _planRepository = planRepository;
            _userService = userService;
        }
        public async Task<CreatePlanResponse> Handle(CreatePlanCommand request, CancellationToken cancellationToken)
        {
            Plan plan = new()
            {
                Id = new Guid(),
                Name = request.Name,
                Price = request.Price,
                DurationInDays = request.DurationInDays,
                StartDate = request.StartDate,
                ExpieryDate = request.ExpieryDate,
                TempDurationInDays = request.TempDurationInDays,
                MaxPropertiesCountAtSameTime = request.MaxPropertiesCountAtSameTime,
                MaxStardPropertiesCountAtSameTime = request.MaxStardPropertiesCountAtSameTime,
                MaxImagesCount = request.MaxImagesCount,
                MaxVideosCount = request.MaxVideosCount,
                IsDisabled = false,
                DefaultPlan = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = _userService.FindUserByContext(),
            };

            await _planRepository.AddPlanAsync(plan);
            
            return new CreatePlanResponse { Success = true, Message = "Plan created successfully." };
        }
    }
}
