using Abwaab.Application.Common.Constants;
using Abwaab.Application.Features.Plans.CancelPlan;
using Abwaab.Application.Features.Plans.CreatePlan;
using Abwaab.Application.Features.Plans.GetAllPlans;
using Abwaab.Application.Features.Users.Profile.Plans.Cancel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Abwaab.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlanController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PlanController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("All-plans")]
        public async Task<IActionResult> GetAllPlans()
        {
            var result = await _mediator.Send(new GetAllPlansQuery());
            return Ok(result);
        }

        [Authorize(Roles = RoleConstants.ROLE_ADMIN)]
        [HttpPost("create-plan")]
        public async Task<IActionResult> CreatePlan([FromBody] CreatePlanCommand plan)
        {
            var result = await _mediator.Send(plan);
            return Ok(result);
        }
        
        [HttpPost("cancel-plan")]
        public async Task<IActionResult> CancelPlan([FromBody] CancelUserPlanCommand plan)
        {
            var result = await _mediator.Send(plan);
            return Ok(result);
        }
    }
}
