using Abwaab.Application.Features.Properties.Queries.GetUserPropertiesSummaryList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Abwaab.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetUserPropertiesSummary")]
        public async Task<IActionResult> GetUserPropertiesSummary()
        {
            List<GetUserPropertySummaryResponse> result = await _mediator.Send(new GetUserPropertySummaryQuery());
            return Ok(result);
        }
    }
}
