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
                   .HasMaxLength(1000);

            builder.Property(p => p.Price)
                   .HasPrecision(18, 2);

            builder.Property(p => p.AreaInSquareMeter)
                   .HasPrecision(18, 2);

            builder.Property(p => p.IsStard)
                   .HasDefaultValue(false);

            builder.Property(p => p.Note)
                .HasMaxLength(500);

            // Relationships
            builder.HasOne(p => p.UserPlan)
                   .WithMany(u => u.Properties)
                   .HasForeignKey(p => p.UserPlandId)
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

            // Indexes
            builder.HasIndex(p => p.UserPlandId);
            builder.HasIndex(p => p.PropertyTypeId);
            builder.HasIndex(p => p.PropertyStateId);
        }
    }
}