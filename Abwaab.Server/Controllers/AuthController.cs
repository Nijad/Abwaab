using Abwaab.Application.DTOs.ApplicationUser;
using Abwaab.Domain.Enums;
using Abwaab.Infrastructure.Common;
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

        [HttpPost("LoginUser")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserRequest loginRequest)
        {
            if (loginRequest == null)
                return BadRequest();

            var response = await _mediator.Send(loginRequest);

            return Ok(response);
        }

        [HttpPost("ResendCode")]
        public async Task<IActionResult> ResendCode([FromBody] ResendCodeRequest resendCodeRequest)
        {
            if (resendCodeRequest == null)
                return BadRequest();

            ResendCodeDTO resendCodeDTO = new ResendCodeDTO
            {
                Identifier = resendCodeRequest.Identifier
            };

            //if(CommonValidation.IsValidEmail(resendCodeDTO.Identifier))
            //    resendCodeDTO.IdentifierType = IdentifierEnum.email;
            //else if(CommonValidation.IsValidPhoneNumber(resendCodeDTO.Identifier))
            //    resendCodeDTO.IdentifierType = IdentifierEnum.phone_number;
            //else
            //    return BadRequest("Invalid identifier. Please provide a valid email or phone number.");

            var response = await _mediator.Send(resendCodeDTO);
            return Ok(response);
        }
    }
}
