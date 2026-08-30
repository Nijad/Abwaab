using MediatR;

namespace Abwaab.Application.Features.Appointments.Commands.Cancel;

public class CancelAppointmentCommand : IRequest<CancelAppointmentResponse>
{
    public Guid AppointmentId { get; set; }
    public string Comment { get; set; } = string.Empty;
}
