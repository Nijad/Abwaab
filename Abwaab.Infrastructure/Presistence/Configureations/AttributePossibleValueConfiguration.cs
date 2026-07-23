using Abwaab.Domain.Entities.PropertyEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abwaab.Infrastructure.Presistence.Configureations
{
    public class AttributePossibleValueConfiguration : IEntityTypeConfiguration<AttributePossibleValue>
    {
        public void Configure(EntityTypeBuilder<AttributePossibleValue> builder)
        {
            builder.ToTable("AttributePossibleValues");

            builder.HasKey(apv => apv.Id);

            builder.Property(apv => apv.Value)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.HasIndex(apv => apv.Value);
        }
    }
}