using Abwaab.Domain.Entities.PropertyEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abwaab.Infrastructure.Presistence.Configureations
{
    public class AttributeConfiguration : IEntityTypeConfiguration<Domain.Entities.PropertyEntities.Attribute>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.PropertyEntities.Attribute> builder)
        {
            builder.ToTable("Attributes");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.AttributeName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasOne(x => x.AttributeDataType)
                .WithMany(x => x.Attributes)
                .HasForeignKey(x=>x.AttributeDataTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(a => a.AttributeName).IsUnique();
            builder.HasIndex(a => a.AttributeDataTypeId);
        }
    }
}