using Abwaab.Domain.Entities.AppointmentEntities;
using Abwaab.Domain.Entities.MediaEntities;
using Abwaab.Domain.Entities.PaymentEntities;
using Abwaab.Domain.Entities.UserEntities;

namespace Abwaab.Domain.Entities.PropertyEntities
{
    public class Property : BaseEntity
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal AreaInSquareMeter { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; } = null!;
        public decimal Price { get; set; }
        public bool IsStard { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public Guid UserId { get; set; }
        public PropertyType PropertyType { get; set; } = null!;
        public Guid PropertyTypeId { get; set; }
        public PropertyState PropertyState { get; set; } = null!;
        public Guid PropertyStateId { get; set; }
        public Finishing Finishing { get; set; } = null!;
        public Guid FinishingId { get; set; }
        public Advertisment? Advertisment { get; set; }
        public Guid? AdvertismentId { get; set; }
        public List<Media>? MediaList { get; set; }
        public List<Appointment>? Appointments { get; set; }
        public List<TimeSlot>? TimeSlots { get; set; }
        public List<PropertyAttribute>? PropertyAttributes { get; set; }
        public List<Payment>? Payments { get; set; }
    }
}
