using MediatR;

namespace Abwaab.Application.Features.Appointments.Queries.GetPropertyAppointments;

public class PropertyAppointmentsQuery : IRequest<PropertyAppointmentsResponse>
{
    public Guid PropertyId { get; set; }
}