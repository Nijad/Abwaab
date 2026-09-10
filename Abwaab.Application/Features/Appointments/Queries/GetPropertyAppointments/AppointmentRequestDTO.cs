namespace Abwaab.Application.Features.Appointments.Queries.GetPropertyAppointments;

public class AppointmentRequestDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Identifier { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public string StateName { get; set; } = string.Empty;
    public string ArabicStateName { get; set; } = string.Empty;
}
