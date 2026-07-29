using Abwaab.Application.DTOs.Profile.NotificationWayUnsubscription;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Abwaab.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProfileController> _logger;
        public ProfileController(
            IMediator mediator,
            ILogger<ProfileController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("SubscribeNotificationWay")]
        public async Task<IActionResult> SubscribeNotificationWay([FromBody] NotificationWaySubsciptionCommand request)
        {
            if (request == null)
                return BadRequest();

            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}
