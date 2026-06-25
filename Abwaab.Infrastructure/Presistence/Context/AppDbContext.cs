using Abwaab.Domain.Entities.AppointmentEntities;
using Abwaab.Domain.Entities.MediaEntities;
using Abwaab.Domain.Entities.NotificationEntities;
using Abwaab.Domain.Entities.PaymentEntities;
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Domain.Entities.UserEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Abwaab.Infrastructure.Presistence.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser,IdentityRole<Guid>, Guid>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentState> AppointmentStates { get; set; }
        public DbSet<AppointmentAction> AppointmentActions { get; set; }

        public DbSet<Advertisment> Advertisments { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<MediaType> MediaTypes { get; set; }


        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationState> NotificationStates { get; set; }
        public DbSet<NotificationWay> NotificationWays { get; set; }
        public DbSet<UserNotificationSubscription> UserNotificationSubscriptions { get; set; }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentState> PaymentStates { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }

        public DbSet<Domain.Entities.PropertyEntities.Attribute> Attributes { get; set; }
        public DbSet<AttributePossibleValue> AttributePossibleValues { get; set; }
        public DbSet<Finishing> Finishings { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyAction> PropertieActions { get; set; }
        public DbSet<PropertyAttribute> PropertyAttributes { get; set; }
        public DbSet<PropertyState> PropertyStates { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }

        public DbSet<OTP> OTPs { get; set; }
        public DbSet<Plan> Plans { get; set; }

    }
}
