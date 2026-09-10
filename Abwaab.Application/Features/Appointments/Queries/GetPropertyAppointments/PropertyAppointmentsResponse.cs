namespace Abwaab.Application.Features.Appointments.Queries.GetPropertyAppointments;

public class PropertyAppointmentsResponse
{
    public Guid PropertyId { get; set; }
    public string PropertyTitle { get; set; } = string.Empty;
    public string PropertyType { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public string CoverPath { get; set; } = string.Empty;
    public List<AppointmentRequestDTO> Requests { get; set; } = new List<AppointmentRequestDTO>();
}
