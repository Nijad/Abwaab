namespace Abwaab.Application.Features.Appointments.Queries.GetUserAppointments;

public class GetUserAppointmentsResponse
{
    public Guid AppointmentId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public TimeOnly EndTime { get; set; }
    public string AppointmentState { get; set; } = string.Empty;
    public string AppointmentDirection { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNo { get; set; } = string.Empty;
    public bool Cancelable { get; set; }
    public string Comments { get; set; } = string.Empty;
    public Guid PropertyId { get; set; }
    public string PropertyTitle { get; set; } = string.Empty;
    public string CoverPath { get; set; } = string.Empty;
}
