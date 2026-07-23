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

            builder.HasIndex(a => a.AttributeName).IsUnique();
        }
    }
}