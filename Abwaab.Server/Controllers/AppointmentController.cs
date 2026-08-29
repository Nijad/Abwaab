using Abwaab.Application.Features.Appointments.Book;
using Abwaab.Application.Features.Users.Auth.Register;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Abwaab.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AppointmentController : ControllerBase
{
    private readonly IMediator _mediator;

    public AppointmentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("BookAppointment")]
    public async Task<IActionResult> BookAppointment([FromBody] BookAppointmentCommand command)
    {
        if (command == null)
            return BadRequest();


        BookAppointmentResponse response = await _mediator.Send(command);

        return Ok(response);
    }
}
