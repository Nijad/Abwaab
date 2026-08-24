using Abwaab.Domain.Entities.PropertyEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abwaab.Infrastructure.Presistence.Configureations
{
    public class TimeSlotConfiguration : IEntityTypeConfiguration<TimeSlot>
    {
        public void Configure(EntityTypeBuilder<TimeSlot> builder)
        {
            builder.ToTable("TimeSlots");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Day)
                .IsRequired();

            builder.Property(t => t.StartTime)
                   .IsRequired();

            builder.Property(t => t.EndTime)
                   .IsRequired();

            // Relationship: TimeSlot -> Property (assumes TimeSlot.Property and TimeSlot.PropertyId exist)
            builder.HasOne(t => t.Property)
                   .WithMany(p => p.TimeSlots)
                   .HasForeignKey(t => t.PropertyId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(t => t.PropertyId);
        }
    }
}