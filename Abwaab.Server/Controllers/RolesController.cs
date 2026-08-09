using Abwaab.Application.Common.Constants;
using Abwaab.Application.Features.Users.Role.AddUserToRole;
using Abwaab.Application.Features.Users.Role.GetAllRoles;
using Abwaab.Application.Features.Users.Role.GetUserRoles;
using Abwaab.Application.Features.Users.Role.RemoveUserFromRole;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Abwaab.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = RoleConstants.ROLE_ADMIN)]
    public class RolesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<RolesController> _logger;
        public RolesController(IMediator mediator,
            ILogger<RolesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("add-user-role")]
        public async Task<IActionResult> AddUserToRole([FromBody] AddUserToRoleCommand command)
        {
            AddUserToRoleDTO request = new()
            {
                Identifier = command.Identifier,
                RoleName = command.RoleName
            };
            var result = await _mediator.Send(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("remove-user-role")]
        public async Task<IActionResult> RemoveUserFromRole([FromBody] RemoveUserFromRoleCommand command)
        {
            RemoveUserFromRoleDTO request = new()
            {
                Identifier = command.Identifier,
                RoleName = command.RoleName
            };
            var result = await _mediator.Send(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("user-roles")]
        public async Task<IActionResult> GetUserRoles([FromQuery] string userIdentifier)
        {
            var result = await _mediator.Send(new GetUserRolesDTO { Identifier = userIdentifier });
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("all-roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _mediator.Send(new GetAllRolesQuery());
            return Ok(roles);
        }
    }
}
