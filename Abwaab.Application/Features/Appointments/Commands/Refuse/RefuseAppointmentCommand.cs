using MediatR;

namespace Abwaab.Application.Features.Appointments.Commands.Refuse;

public class RefuseAppointmentCommand : IRequest<RefuseAppointmentResponse>
{
    public Guid AppointmentId { get; set; }
    public string Comment { get; set; } = string.Empty;
}
