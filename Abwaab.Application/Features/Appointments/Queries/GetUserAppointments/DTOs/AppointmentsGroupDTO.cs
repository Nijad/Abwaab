namespace Abwaab.Application.Features.Appointments.Queries.GetUserAppointments.DTOs;

public class AppointmentsGroupDTO
{
    public DateOnly AppointmentDate { get; set; }
    public string DayName { get; set; } = string.Empty;
    public List<AppointmentDetailsDTO>? Appointments { get; set; }
}
