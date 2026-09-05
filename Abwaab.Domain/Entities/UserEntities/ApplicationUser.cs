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
        public List<UserPlan>? UserPlans { get; set; }
        public List<UserNotificationSubscription>? NotificationWaySubscriptions { get; set; }
        public List<Appointment>? Appointments { get; set; }
        public string? PreviousEmail { get; set; }
        public string? PreviousPhoneNumber { get; set; }
    }
}
