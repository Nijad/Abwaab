using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Domain.Entities.UserEntities;

namespace Abwaab.Domain.Entities.AppointmentEntities
{
    public class Appointment : BaseEntity
    {
        public Property Property { get; set; } = null!;
        public Guid PropertyId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public TimeOnly EndTime { get; set; }
        public AppointmentState AppointmentState { get; set; } = null!;
        public Guid AppointmentStateId { get; set; }
        public string? UserComments { get; set; }
    }
}