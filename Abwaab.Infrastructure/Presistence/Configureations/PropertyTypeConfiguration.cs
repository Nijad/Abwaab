using Abwaab.Domain.Entities.PropertyEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abwaab.Infrastructure.Presistence.Configureations
{
    public class PropertyTypeConfiguration : IEntityTypeConfiguration<PropertyType>
    {
        public void Configure(EntityTypeBuilder<PropertyType> builder)
        {
            builder.ToTable("PropertyTypes");

            builder.HasKey(pt => pt.Id);

            builder.Property("TypeName")
                   .IsRequired()
                   .HasMaxLength(200);

            // Relationship -> Property (assumes Property.PropertyTypeId exists)
            //builder.HasMany<Property>(pt => new List<Property>())
            //       .WithOne()
            //       .HasForeignKey("PropertyTypeId")
            //       .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex("TypeName").IsUnique();
        }
    }
}