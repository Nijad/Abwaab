using Abwaab.Domain.Entities.AppointmentEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abwaab.Infrastructure.Presistence.Configureations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.ToTable("Appointments");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Date)
                   .IsRequired();

            builder.Property(a => a.UserComments)
                   .HasMaxLength(2000);

            // Relationships
            builder.HasOne(a => a.Property)
                   .WithMany(p => p.Appointments)
                   .HasForeignKey(a => a.PropertyId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.User)
                   .WithMany(u => u.Appointments)
                   .HasForeignKey(a => a.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.AppointmentState)
                   .WithMany(s => s.Appointments)
                   .HasForeignKey(a => a.AppointmentStateId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(a => a.PropertyId);
            builder.HasIndex(a => a.UserId);
            builder.HasIndex(a => a.AppointmentStateId);
            builder.HasIndex(a => a.Date);
        }
    }
}