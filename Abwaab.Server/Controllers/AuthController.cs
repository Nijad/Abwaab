using Abwaab.Application.DTOs.ApplicationUser.ChangePassword;
using Abwaab.Application.DTOs.ApplicationUser.ForgotPassword;
using Abwaab.Application.DTOs.ApplicationUser.IdentifierManagement;
using Abwaab.Application.DTOs.ApplicationUser.LoginUser;
using Abwaab.Application.DTOs.ApplicationUser.LogoutUser;
using Abwaab.Application.DTOs.ApplicationUser.RefreshToken;
using Abwaab.Application.DTOs.ApplicationUser.RegisterUser;
using Abwaab.Application.DTOs.ApplicationUser.VerificationCode;
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
        public async Task<IActionResult> ResendCode([FromBody] ResendCodeCommand resendCodeRequest)
        {
            if (resendCodeRequest == null)
                return BadRequest();

            ResendCodeDTO resendCodeDTO = new ResendCodeDTO
            {
                Identifier = resendCodeRequest.Identifier
            };

            var response = await _mediator.Send(resendCodeDTO);
            return Ok(response);
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand forgotPasswordRequest)
        {
            if (forgotPasswordRequest == null)
                return BadRequest();

            ForgotPasswordDTO forgotPasswordDTO = new ForgotPasswordDTO
            {
                Identifier = forgotPasswordRequest.Identifier,
                ConfirmNewPassword = forgotPasswordRequest.ConfirmNewPassword,
                NewPassword = forgotPasswordRequest.NewPassword
            };

            var response = await _mediator.Send(forgotPasswordDTO);
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
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand request)
        {
            if (request == null)
                return BadRequest();
            ChangePasswordDTO resetPasswordDTO = new ChangePasswordDTO
            {
                CurrentPassword = request.CurrentPassword,
                NewPassword = request.NewPassword,
                ConfirmNewPassword = request.ConfirmPassword
            };
            var response = await _mediator.Send(resetPasswordDTO);
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

        [HttpPost("initiate-email-change")]
        public async Task<IActionResult> InitiateEmailChange([FromBody] InitiateEmailChangeCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("confirm-email-change")]
        public async Task<IActionResult> ConfirmEmailChange([FromBody] ConfirmEmailChangeCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("initiate-phone-change")]
        public async Task<IActionResult> InitiatePhoneChange([FromBody] InitiatePhoneNoChangeCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("confirm-phone-change")]
        public async Task<IActionResult> ConfirmPhoneChange([FromBody] ConfirmPhoneNoChangeCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("cancel-email-change")]
        public async Task<IActionResult> CancelEmailChange()
        {
            var result = await _mediator.Send(new CancelEmailChangeCommand());
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("cancel-phone-change")]
        public async Task<IActionResult> CancelPhoneChange()
        {
            var result = await _mediator.Send(new CancelPhoneChangeCommand());
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
