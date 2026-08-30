using MediatR;

namespace Abwaab.Application.Features.Appointments.Commands.Complete;

public class CompleteAppointmentCommand : IRequest<CompleteAppointmentResponse>
{
    public Guid AppointmentId { get; set; }
    public string Comment { get; set; } = string.Empty;
}
