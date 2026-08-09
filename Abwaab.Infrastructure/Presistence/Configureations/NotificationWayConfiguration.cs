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

            builder.HasIndex(w => w.WayName).IsUnique();
        }
    }
}