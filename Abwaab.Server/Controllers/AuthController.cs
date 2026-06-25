using Abwaab.Application.Common.Contracts;
using Abwaab.Application.DTOs.ApplicationUser;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Abwaab.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("LoginUserByEmail")]
        public async Task<IActionResult> LoginUserByEmail([FromBody] LoginUserByEmailRequest loginRequest)
        {
            if (loginRequest == null)
                return BadRequest();

            var response = await _mediator.Send(loginRequest);

            return Ok(response);
        }

        [HttpPost("RegisterUserByEmail")]
        public async Task<IActionResult> RegisterUserByEmail([FromBody] RegisterUserByEmailRequest registerRequest)
        {
            if (registerRequest == null)
                return BadRequest();

            RegisterUserResponse response = await _mediator.Send(registerRequest);

            return Ok(response);
        }
    }
}
