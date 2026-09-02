using Abwaab.Application.Features.Appointments.Queries.GetUserAppointments.DTOs;

namespace Abwaab.Application.Features.Appointments.Queries.GetUserAppointments;

public class GetUserAppointmentsResponse
{
    public List<AppointmentsGroupDTO>? ReceivedAppointments { get; set; }
    public List<AppointmentsGroupDTO>? RequestedAppointments { get; set; }
}
