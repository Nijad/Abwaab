using Abwaab.Domain.Entities.AppointmentEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abwaab.Infrastructure.Presistence.Configureations
{
    public class AppointmentStateConfiguration : IEntityTypeConfiguration<AppointmentState>
    {
        public void Configure(EntityTypeBuilder<AppointmentState> builder)
        {
            builder.ToTable("AppointmentStates");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.StateName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasIndex(s => s.StateName).IsUnique();
        }
    }
}