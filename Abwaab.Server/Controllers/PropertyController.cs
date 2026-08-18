using Abwaab.Application.Features.Notifications.AllNotificationWays;
using Abwaab.Application.Features.Properties.Queries.GetFinishingList;
using Abwaab.Application.Features.Properties.Queries.GetPropertyTypesList;
using Abwaab.Application.Features.Properties.Update;
using Abwaab.Application.Features.Properties.Update.BasicInfo;
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

        // 1. add property (Prepare to start)
        [HttpPost("add-property")]
        public async Task<IActionResult> AddProperty()
        {
            var result = await _mediator.Send(new AddPropertyCommand());
            return Ok(result);
        }

        //2.1.  update basic information
        [HttpPut("update-property")]
        public async Task<IActionResult> UpdateProperty([FromBody] UpdatePropertyBasicInfoCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        //2.2.  add media
        //2.3.  delete media
        //2.4.  add time slot
        //2.5.  delete time slot
        //2.6.  add attribute
        //2.6.  delete attribure

        //2.7.  star property
        //2.8.  unstar propery


        //3. reject property/temp will be rejected for month then will be deleted automatically
        //      if owner modify property/temp will turn pending again

        //4. approve property will turn it to published
        //      if it was temp original one will be deleted and detach from user plan 
        //          and temp turns to published

        //  another scenario: reflect modification to original and delete temp (which is easier)

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
    }
}
