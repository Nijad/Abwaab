namespace Abwaab.Application.Features.Appointments.Queries.GetUserAppointments;

public class AppointmentDetailsDTO
{
    //appointment details
    public Guid AppointmentId { get; set; }
    public TimeOnly FromTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string AppointmentState { get; set; } = string.Empty;
    public string AppointmentDirection { get; set; } = string.Empty;
    public bool Cancelable { get; set; }
    public string Comments { get; set; } = string.Empty;

    //second party details
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNo { get; set; } = string.Empty;

    //property details
    public Guid PropertyId { get; set; }
    public string PropertyTitle { get; set; } = string.Empty;
    public string CoverPath { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Area { get; set; }
    public decimal Price { get; set; }
}
