using Abwaab.Domain.Entities.PropertyEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abwaab.Infrastructure.Presistence.Configureations
{
    public class AttributeDataTypeConfiguration : IEntityTypeConfiguration<AttributeDataType>
    {
        public void Configure(EntityTypeBuilder<AttributeDataType> builder)
        {
            builder.ToTable("AttributeDataTypes");

            builder.HasKey(a => a.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(p => p.Name).IsUnique();
        }
    }
}
