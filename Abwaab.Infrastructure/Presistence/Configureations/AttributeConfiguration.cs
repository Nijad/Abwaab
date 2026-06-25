using Abwaab.Domain.Entities.PropertyEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abwaab.Infrastructure.Presistence.Configureations
{
    public class AttributeConfiguration : IEntityTypeConfiguration<Abwaab.Domain.Entities.PropertyEntities.Attribute>
    {
        public void Configure(EntityTypeBuilder<Abwaab.Domain.Entities.PropertyEntities.Attribute> builder)
        {
            builder.ToTable("Attributes");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.AttributeName)
                   .IsRequired()
                   .HasMaxLength(200);

            //builder.HasMany(a => a.PropertyAttributes)
            //       .WithOne(pa => pa.Attribute)
            //       .HasForeignKey(pa => pa.AttributeId)
            //       .OnDelete(DeleteBehavior.Restrict);

            //builder.HasMany(a => a.PossibleValues)
            //       .WithOne(pv => pv.Attribute)
            //       .HasForeignKey(pv => pv.AttributeId)
            //       .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(a => a.AttributeName).IsUnique();
        }
    }
}