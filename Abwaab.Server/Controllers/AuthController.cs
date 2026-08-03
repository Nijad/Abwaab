using Abwaab.Application.Features.Users.Auth.Login;
using Abwaab.Application.Features.Users.Auth.Logout;
using Abwaab.Application.Features.Users.Auth.RefreshToken;
using Abwaab.Application.Features.Users.Auth.Register;
using Abwaab.Application.Features.Users.Auth.SendCode;
using Abwaab.Application.Features.Users.Auth.VerificationCode;
using Abwaab.Application.Features.Users.Profile.Password.Forgot;
using Abwaab.Application.Features.Users.Profile.Password.Reset;
using Abwaab.Application.Features.Users.Profile.Password.VerifyResetCode;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Abwaab.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IMediator mediator,
            ILogger<AuthController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand registerRequest)
        {
            if (registerRequest == null)
                return BadRequest();

            RegisterUserDTO registerDTO = new RegisterUserDTO
            {
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Identifier = registerRequest.Identifier,
                Password = registerRequest.Password,
                ConfirmPassword = registerRequest.ConfirmPassword
            };

            RegisterUserResponse response = await _mediator.Send(registerDTO);

            return Ok(response);
        }

        [HttpPost("VerifyAccount")]
        public async Task<IActionResult> VerifyAccount([FromBody] VerifyCodeCommand verifyCodeRequest)
        {
            if (verifyCodeRequest == null)
                return BadRequest();
            VerifyCodeDTO verifyCodeDTO = new VerifyCodeDTO
            {
                Identifier = verifyCodeRequest.Identifier,
                Code = verifyCodeRequest.Code
            };

            var response = await _mediator.Send(verifyCodeDTO);
            return Ok(response);
        }

        [HttpPost("LoginUser")]
        [EnableRateLimiting("LoginPolicy")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserCommand loginRequest)
        {
            if (loginRequest == null)
                return BadRequest();

            LoginUserDTO loginUserDTO = new LoginUserDTO
            {
                Identifier = loginRequest.Identifier,
                Password = loginRequest.Password
            };

            var response = await _mediator.Send(loginUserDTO);

            return Ok(response);
        }

        [HttpPost("ResendCode")]
        public async Task<IActionResult> ResendCode([FromBody] SendCodeCommand resendCodeRequest)
        {
            if (resendCodeRequest == null)
                return BadRequest();

            SendCodeDTO resendCodeDTO = new SendCodeDTO
            {
                Identifier = resendCodeRequest.Identifier
            };

            var response = await _mediator.Send(resendCodeDTO);
            return Ok(response);
        }
             
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand refreshTokenRequest)
        {
            if (refreshTokenRequest == null)
                return BadRequest();
            var response = await _mediator.Send(refreshTokenRequest);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutCommand request)
        {
            if (request == null)
                return BadRequest();

            LogoutResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand forgotPasswordRequest)
        {
            if (forgotPasswordRequest == null)
                return BadRequest();

            ForgotPasswordDTO forgotPasswordDTO = new ForgotPasswordDTO
            {
                Identifier = forgotPasswordRequest.Identifier
            };

            var response = await _mediator.Send(forgotPasswordDTO);
            return Ok(response);
        }

        [HttpPost("verify-reset-code")]
        public async Task<IActionResult> VerifyResetCode([FromBody] VerifyResetCodeCommand command)
        {
            VerifyResetCodeDTO request = new()
            {
                Identifier = command.Identifier,
                Code = command.Code
            };

            var result = await _mediator.Send(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            ResetPasswordDTO request = new()
            {
                Identifier = command.Identifier,
                Code = command.Code,
                NewPassword = command.NewPassword,
                ConfirmNewPassword = command.ConfirmNewPassword
            };
            var result = await _mediator.Send(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
