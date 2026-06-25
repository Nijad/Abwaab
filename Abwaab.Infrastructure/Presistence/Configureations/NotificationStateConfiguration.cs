using Abwaab.Domain.Entities.NotificationEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abwaab.Infrastructure.Presistence.Configureations
{
    public class NotificationStateConfiguration : IEntityTypeConfiguration<NotificationState>
    {
        public void Configure(EntityTypeBuilder<NotificationState> builder)
        {
            builder.ToTable("NotificationStates");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.StateName)
                   .IsRequired()
                   .HasMaxLength(200);

            //builder.HasMany(s => s.Notifications)
            //       .WithOne(n => n.NotificationState)
            //       .HasForeignKey(n => n.NotificationStateId)
            //       .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(s => s.StateName).IsUnique();
        }
    }
}