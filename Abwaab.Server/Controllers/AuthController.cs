using Abwaab.Application.DTOs.ApplicationUser;
using MediatR;
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

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest registerRequest)
        {
            if (registerRequest == null)
                return BadRequest();

            RegisterUserResponse response = await _mediator.Send(registerRequest);

            return Ok(response);
        }

        [HttpPost("VerifyAccount")]
        public async Task<IActionResult> VerifyAccount([FromBody] VerifyCodeRequest verifyCodeRequest)
        {
            if (verifyCodeRequest == null)
                return BadRequest();
            var response = await _mediator.Send(verifyCodeRequest);
            return Ok(response);
        }

        [HttpPost("LoginUserByEmail")]
        public async Task<IActionResult> LoginUserByEmail([FromBody] LoginUserRequest loginRequest)
        {
            if (loginRequest == null)
                return BadRequest();

            var response = await _mediator.Send(loginRequest);

            return Ok(response);
        }
    }
}
