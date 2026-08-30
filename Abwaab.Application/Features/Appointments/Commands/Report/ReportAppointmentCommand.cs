using MediatR;

namespace Abwaab.Application.Features.Appointments.Commands.Report;

public class ReportAppointmentCommand : IRequest<ReportAppointmentResponse>
{
    public Guid AppointmentId { get; set; }
    public string Comment { get; set; } = string.Empty;
}
