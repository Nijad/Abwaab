using Abwaab.Domain.Entities.PropertyEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abwaab.Infrastructure.Presistence.Configureations
{
    public class PropertyAttributeConfiguration : IEntityTypeConfiguration<PropertyAttribute>
    {
        public void Configure(EntityTypeBuilder<PropertyAttribute> builder)
        {
            builder.ToTable("PropertyAttributes");

            builder.HasKey(pa => pa.Id);

            builder.Property(pa => pa.AttributeValue)
                   .IsRequired()
                   .HasMaxLength(1000);

            builder.HasOne(pa => pa.Property)
                   .WithMany(p => p.PropertyAttributes)
                   .HasForeignKey(pa => pa.PropertyId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pa => pa.Attribute)
                   .WithMany(a => a.PropertyAttributes)
                   .HasForeignKey(pa => pa.AttributeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(pa => pa.PropertyId);
            builder.HasIndex(pa => pa.AttributeId);
        }
    }
}