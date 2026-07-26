using Abwaab.Domain.Entities.AppointmentEntities;
using Abwaab.Domain.Entities.NotificationEntities;
using Microsoft.AspNetCore.Identity;

namespace Abwaab.Domain.Entities.UserEntities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public int ReportCount { get; set; }
        public bool IsBlocked { get; set; }
        //public Guid PlanId { get; set; }
        //public Plan Plan { get; set; } = null!;
        public List<UserPlan>? UserPlans { get; set; }
        public DateTime? PlanExpieryDate { get; set; }
        //public List<Payment>? Payments { get; set; }
        public List<UserNotificationSubscription>? NotificationWaySubscriptions { get; set; }
        public List<Appointment>? Appointments { get; set; }
        public List<OTP>? OTPs { get; set; }
        //public List<Property>? Properties { get; set; }
        public string? PreviousEmail { get; set; }
        public string? PreviousPhoneNumber { get; set; }
    }
}
