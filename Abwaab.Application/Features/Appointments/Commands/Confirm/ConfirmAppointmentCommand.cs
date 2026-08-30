using MediatR;

namespace Abwaab.Application.Features.Appointments.Commands.Confirm;

public class ConfirmAppointmentCommand : IRequest<ConfirmAppointmentResponse>
{
    public Guid AppointmentId { get; set; }
}
