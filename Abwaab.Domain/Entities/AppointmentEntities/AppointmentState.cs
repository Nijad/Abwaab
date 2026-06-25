namespace Abwaab.Domain.Entities.AppointmentEntities
{
    public class AppointmentState : BaseEntity
    {
        public string StateName { get; set; } = null!;
        public List<Appointment>? Appointments { get; set; }
    }
}