using Abwaab.Domain.Entities.NotificationEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abwaab.Infrastructure.Presistence.Configureations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");

            builder.HasKey(n => n.Id);

            builder.Property(n => n.Title)
                   .HasMaxLength(500);

            builder.Property(n => n.Message)
                   .IsRequired()
                   .HasMaxLength(4000);

            builder.Property(n => n.IsRead)
                   .IsRequired();

            builder.Property(n => n.ResponseNote)
                   .HasMaxLength(2000);

            // Relationship -> UserNotificationSubscription (recipient / subscription)
            builder.HasOne(n => n.NotificationSubscription)
                   .WithMany(t => t.Notifications)
                   .HasForeignKey(n => n.NotificationSubscriptionId)
                   .OnDelete(DeleteBehavior.Restrict);

            //Relationship->NotificationState
            builder.HasOne(n => n.NotificationState)
                   .WithMany(s => s.Notifications)
                   .HasForeignKey(n => n.NotificationStateId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Indexes for fast lookups
            builder.HasIndex(n => n.NotificationSubscriptionId);
            builder.HasIndex(n => n.NotificationStateId);
            builder.HasIndex(n => n.IsRead);
            builder.HasIndex(n => n.Title);
        }
    }
}