using Abwaab.Domain.Entities.NotificationEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abwaab.Infrastructure.Presistence.Configureations
{
    public class NotificationWayConfiguration : IEntityTypeConfiguration<NotificationWay>
    {
        public void Configure(EntityTypeBuilder<NotificationWay> builder)
        {
            builder.ToTable("NotificationWays");

            builder.HasKey(w => w.Id);

            builder.Property(w => w.WayName)
                   .IsRequired()
                   .HasMaxLength(200);

            //builder.HasMany(n => n.NotificationSuscriptions)
            //       .WithOne(ns => ns.NotificationWay)
            //       .HasForeignKey(ns => ns.NotificationWayId)
            //       .OnDelete(DeleteBehavior.Cascade);

            // Navigation collections in domain may vary — keep mapping minimal here.
            // If NotificationWay should have a navigation to Notifications or Subscriptions,
            // update this configuration once the domain navigation property is present.

            builder.HasIndex(w => w.WayName).IsUnique();
        }
    }
}