namespace Abwaab.Application.Features.Appointments.Queries.GetUserAppointments;

public class GetUserAppointmentsResponse
{
    public DateOnly AppointmentDate { get; set; }
    public string DayName { get; set; } = string.Empty;
    public List<AppointmentDetailsDTO>? Appointments { get; set; }
}
