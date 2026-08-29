using Abwaab.Application.Common.Constants;
using Abwaab.Application.Features.Properties.Accept;
using Abwaab.Application.Features.Properties.Enable;
using Abwaab.Application.Features.Properties.Queries.AvailableTimeSlots;
using Abwaab.Application.Features.Properties.Queries.GetFinishingList;
using Abwaab.Application.Features.Properties.Queries.GetPropertyDetails;
using Abwaab.Application.Features.Properties.Queries.GetPropertyForUpdate;
using Abwaab.Application.Features.Properties.Queries.GetPropertyTypesList;
using Abwaab.Application.Features.Properties.Queries.UserProperties;
using Abwaab.Application.Features.Properties.Reject;
using Abwaab.Application.Features.Properties.Star;
using Abwaab.Application.Features.Properties.Submit;
using Abwaab.Application.Features.Properties.Unstar;
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
        [Authorize(Roles = RoleConstants.ROLE_USER)]
        public async Task<IActionResult> AddProperty()
        {
            var result = await _mediator.Send(new AddPropertyCommand());
            return Ok(result);
        }

        [HttpGet("GetPropertyForUpdate")]
        public async Task<IActionResult> GetPropertyForUpdate(Guid propertyId)
        {
            PropertyForUpdateQuery query = new() { PropertyId = propertyId};
            PropertyForUpdateResponse result = await _mediator.Send(query);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("PropertyDetails")]
        public async Task<IActionResult> PropertyDetails(Guid propertyId)
        {
            PropertyDetailsQuery query = new() { PropertyId = propertyId};
            PropertyDetailsResponse result = await _mediator.Send(query);
            return Ok(result);
        }

        
        [HttpGet("PropertyTimeSlots")]
        public async Task<IActionResult> PropertyTimeSlots(Guid propertyId)
        {
            AvailableTimeSlotsQuery query = new() { PropertyId = propertyId};
            List<AvailableTimeSlotsResponse> result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("update-property")]
        [Authorize(Roles = RoleConstants.ROLE_USER)]
        public async Task<IActionResult> UpdateProperty([FromBody] UpdatePropertyCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("submit-property")]
        [Authorize(Roles = RoleConstants.ROLE_USER)]
        public async Task<IActionResult> SubmitProperty([FromBody] SubmitPropertyCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("reject-property")]
        [Authorize(Roles = RoleConstants.ROLE_ADMIN)]
        public async Task<IActionResult> RejectProperty([FromBody] AcceptPropertyCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("accept-property")]
        [Authorize(Roles = RoleConstants.ROLE_ADMIN)]
        public async Task<IActionResult> AcceptProperty([FromBody] AcceptPropertyCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("disable-property")]
        [Authorize(Roles = RoleConstants.ROLE_ADMIN)]
        public async Task<IActionResult> DisableProperty([FromBody] DisablePropertyCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("enable-property")]
        [Authorize(Roles = RoleConstants.ROLE_ADMIN)]
        public async Task<IActionResult> EnableProperty([FromBody] EnablePropertyCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("finishing-list")]
        public async Task<IActionResult> GetFinishingList()
        {
            var result = await _mediator.Send(new FinishingQuery());
            return Ok(result);
        }

        [HttpGet("property-types-list")]
        public async Task<IActionResult> GetPropertyTypesList()
        {
            var result = await _mediator.Send(new PropertyTypeQuery());
            return Ok(result);
        }

        [HttpPost("star-property")]
        public async Task<IActionResult> StarProperty(StarPropertyCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("unstar-property")]
        public async Task<IActionResult> UnstarProperty(UnstarPropertyCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("UserProperties")]
        public async Task<IActionResult> UserProperties()
        {
            List<UserPropertiesResponse> result = await _mediator.Send(new UserPropertiesQuery());
            return Ok(result);
        }
    }
}
