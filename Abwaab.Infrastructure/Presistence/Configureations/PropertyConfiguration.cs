using Abwaab.Domain.Entities.PropertyEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abwaab.Infrastructure.Presistence.Configureations
{
    public class PropertyConfiguration : IEntityTypeConfiguration<Property>
    {
        public void Configure(EntityTypeBuilder<Property> builder)
        {
            builder.ToTable("Properties");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Title)
                   .HasMaxLength(200);

            builder.Property(p => p.Description)
                   .HasMaxLength(4000);

            builder.Property(p => p.Address)
                   .IsRequired()
                   .HasMaxLength(1000);

            builder.Property(p => p.Price)
                   .HasPrecision(18, 2);

            builder.Property(p => p.AreaInSquareMeter)
                   .HasPrecision(18, 2);

            builder.Property(p => p.IsStard)
                   .HasDefaultValue(false);

            // Relationships
            builder.HasOne(p => p.User)
                   .WithMany(u => u.Properties)
                   .HasForeignKey(p => p.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.PropertyType)
                   .WithMany(pt => pt.Properties)
                   .HasForeignKey(p => p.PropertyTypeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.PropertyState)
                   .WithMany(ps => ps.Properties)
                   .HasForeignKey(p => p.PropertyStateId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Finishing)
                   .WithMany(f => f.Properties)
                   .HasForeignKey(p => p.FinishingId)
                   .OnDelete(DeleteBehavior.Restrict);

            //builder.HasMany(p => p.MediaList)
            //       .WithOne(ml => ml.Property)
            //       .HasForeignKey(m => m.PropertyId)
            //       .OnDelete(DeleteBehavior.Cascade);

            //builder.HasMany(p => p.Appointments)
            //       .WithOne(a => a.Property)
            //       .HasForeignKey(a => a.PropertyId)
            //       .OnDelete(DeleteBehavior.Cascade);

            //builder.HasMany(p => p.TimeSlots)
            //       .WithOne(ts => ts.Property)
            //       .HasForeignKey(ts => ts.PropertyId)
            //       .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(p => p.UserId);
            builder.HasIndex(p => p.PropertyTypeId);
            builder.HasIndex(p => p.PropertyStateId);
        }
    }
}