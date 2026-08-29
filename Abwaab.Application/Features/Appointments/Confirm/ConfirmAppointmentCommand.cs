using MediatR;

namespace Abwaab.Application.Features.Appointments.Confirm;

public class ConfirmAppointmentCommand : IRequest<ConfirmAppointmentResponse>
{
    public Guid AppointmentId { get; set; }
}
