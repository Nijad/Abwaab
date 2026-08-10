using Abwaab.Application.Features.Properties.Update;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Abwaab.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PropertyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PropertyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("add-property")]
        public async Task<IActionResult> AddProperty()
        {
            var result = await _mediator.Send(new AddPropertyCommand());
            return Ok(result);
        }
    }
}
