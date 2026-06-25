using Abwaab.Domain.Entities.NotificationEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abwaab.Infrastructure.Presistence.Configureations
{
    public class UserNotificationSubscriptionConfiguration : IEntityTypeConfiguration<UserNotificationSubscription>
    {
        public void Configure(EntityTypeBuilder<UserNotificationSubscription> builder)
        {
            builder.ToTable("UserNotificationSubscriptions");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.IsInactive)
                   .IsRequired();

            builder.HasOne(s => s.User)
                   .WithMany(u => u.NotificationWaySubscriptions)
                   .HasForeignKey(s => s.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.NotificationWay)
                   .WithMany(nw => nw.NotificationSuscriptions)
                   .HasForeignKey(s => s.NotificationWayId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(s => s.UserId);
            builder.HasIndex(s => s.NotificationWayId);
            builder.HasIndex(s => s.IsInactive);
        }
    }
}