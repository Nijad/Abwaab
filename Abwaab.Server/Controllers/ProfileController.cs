using Abwaab.Application.Features.Users.Profile.Email.Cancel;
using Abwaab.Application.Features.Users.Profile.Email.Confirm;
using Abwaab.Application.Features.Users.Profile.Email.InitiateChange;
using Abwaab.Application.Features.Users.Profile.NotificationWaySubscription.Subscribe;
using Abwaab.Application.Features.Users.Profile.NotificationWaySubscription.Unsubscribe;
using Abwaab.Application.Features.Users.Profile.Password.Change;
using Abwaab.Application.Features.Users.Profile.Password.Forgot;
using Abwaab.Application.Features.Users.Profile.Password.Reset;
using Abwaab.Application.Features.Users.Profile.Password.VerifyResetCode;
using Abwaab.Application.Features.Users.Profile.Phone.Cancel;
using Abwaab.Application.Features.Users.Profile.Phone.Confirm;
using Abwaab.Application.Features.Users.Profile.Phone.InitiateChange;
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

        [HttpPost("SubscribeNotificationWay")]
        public async Task<IActionResult> SubscribeNotificationWay([FromBody] NotificationWaySubscriptionCommand request)
        {
            if (request == null)
                return BadRequest();

            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("UnsubscribeNotificationWay")]
        public async Task<IActionResult> UnsubscribeNotificationWay([FromBody] NotificationWayUnsubsciptionCommand request)
        {
            if (request == null)
                return BadRequest();

            var result = await _mediator.Send(request);
            return Ok(result);
        }

    }
}
