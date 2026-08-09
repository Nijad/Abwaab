using Abwaab.Application.Common.Constants;
using Abwaab.Application.Features.Payments.Confirm;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Abwaab.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = RoleConstants.ROLE_ADMIN)]
        [HttpPost("confirm-payment")]
        public async Task<IActionResult> ConfirmPayment([FromBody] ConfirmPaymentCommand command)
        {
            if (command == null)
                return BadRequest();
            ConfirmPaymentResponse response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
