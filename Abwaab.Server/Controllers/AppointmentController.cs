using Abwaab.Application.Features.Appointments.Commands.Book;
using Abwaab.Application.Features.Appointments.Commands.Cancel;
using Abwaab.Application.Features.Appointments.Commands.Complete;
using Abwaab.Application.Features.Appointments.Commands.Confirm;
using Abwaab.Application.Features.Appointments.Commands.Refuse;
using Abwaab.Application.Features.Appointments.Commands.Report;
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

    [HttpPut("ConfirmAppointment")]
    public async Task<IActionResult> ConfirmAppointment([FromBody] ConfirmAppointmentCommand command)
    {
        if (command == null)
            return BadRequest();


        ConfirmAppointmentResponse response = await _mediator.Send(command);

        return Ok(response);
    }

    [HttpPut("CancelAppointment")]
    public async Task<IActionResult> CancelAppointment([FromBody] CancelAppointmentCommand command)
    {
        if (command == null)
            return BadRequest();


        CancelAppointmentResponse response = await _mediator.Send(command);

        return Ok(response);
    }

    [HttpPut("CompleteAppointment")]
    public async Task<IActionResult> CompleteAppointment([FromBody] CompleteAppointmentCommand command)
    {
        if (command == null)
            return BadRequest();


        CompleteAppointmentResponse response = await _mediator.Send(command);

        return Ok(response);
    }

    [HttpPut("RefuseAppointment")]
    public async Task<IActionResult> RefuseAppointment([FromBody] RefuseAppointmentCommand command)
    {
        if (command == null)
            return BadRequest();


        RefuseAppointmentResponse response = await _mediator.Send(command);

        return Ok(response);
    }

    [HttpPut("ReportAppointment")]
    public async Task<IActionResult> ReportAppointment([FromBody] ReportAppointmentCommand command)
    {
        if (command == null)
            return BadRequest();


        ReportAppointmentResponse response = await _mediator.Send(command);

        return Ok(response);
    }
}
