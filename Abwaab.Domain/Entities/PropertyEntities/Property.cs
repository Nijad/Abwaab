using Abwaab.Domain.Entities.AppointmentEntities;
using Abwaab.Domain.Entities.MediaEntities;
using Abwaab.Domain.Entities.UserEntities;

namespace Abwaab.Domain.Entities.PropertyEntities
{
    public class Property : BaseEntity
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal? AreaInSquareMeter { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? Address { get; set; }
        public decimal? Price { get; set; }
        public bool IsStard { get; set; } = false;
        public Guid UserPlandId { get; set; }
        public UserPlan UserPlan { get; set; } = null!;
        public PropertyType? PropertyType { get; set; }
        public Guid? PropertyTypeId { get; set; }
        public PropertyState PropertyState { get; set; } = null!;
        public Guid PropertyStateId { get; set; }
        public Finishing? Finishing { get; set; }
        public Guid? FinishingId { get; set; }
        public List<Media>? MediaList { get; set; }
        public List<Appointment>? Appointments { get; set; }
        public List<TimeSlot>? TimeSlots { get; set; }
        public List<PropertyAttribute>? PropertyAttributes { get; set; }
        public int NumberOfView { get; set; }
    }
}
