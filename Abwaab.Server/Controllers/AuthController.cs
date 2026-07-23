using Abwaab.Application.DTOs.ApplicationUser;
using Abwaab.Application.DTOs.ApplicationUser.ForgotPassword;
using Abwaab.Application.DTOs.ApplicationUser.LoginUser;
using Abwaab.Application.DTOs.ApplicationUser.RefreshToken;
using Abwaab.Application.DTOs.ApplicationUser.RegisterUser;
using Abwaab.Application.DTOs.ApplicationUser.VerificationCode;
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
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest registerRequest)
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
        public async Task<IActionResult> VerifyAccount([FromBody] VerifyCodeRequest verifyCodeRequest)
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
        public async Task<IActionResult> LoginUser([FromBody] LoginUserRequest loginRequest)
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
        public async Task<IActionResult> ResendCode([FromBody] ResendCodeRequest resendCodeRequest)
        {
            if (resendCodeRequest == null)
                return BadRequest();

            IdentifierDTO resendCodeDTO = new IdentifierDTO
            {
                Identifier = resendCodeRequest.Identifier
            };

            var response = await _mediator.Send(resendCodeDTO);
            return Ok(response);
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest forgotPasswordRequest)
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
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            if (refreshTokenRequest == null)
                return BadRequest();
            var response = await _mediator.Send(refreshTokenRequest);
            return Ok(response);
        }

        //[HttpPost("ResetPassword")]
        //public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest resetPasswordRequest)
        //{
        //    if (resetPasswordRequest == null)
        //        return BadRequest();
        //    ResetPasswordDTO resetPasswordDTO = new ResetPasswordDTO
        //    {
        //        Identifier = resetPasswordRequest.Identifier,
        //        Token = resetPasswordRequest.Token,
        //        NewPassword = resetPasswordRequest.NewPassword,
        //        ConfirmNewPassword = resetPasswordRequest.ConfirmNewPassword
        //    };
        //    var response = await _mediator.Send(resetPasswordDTO);
        //    return Ok(response);
        //}
    }
}
