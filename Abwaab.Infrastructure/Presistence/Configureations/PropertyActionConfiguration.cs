using Abwaab.Domain.Entities.PropertyEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abwaab.Infrastructure.Presistence.Configureations
{
    public class PropertyActionConfiguration : IEntityTypeConfiguration<PropertyAction>
    {
        public void Configure(EntityTypeBuilder<PropertyAction> builder)
        {
            builder.ToTable("PropertyActions");

            builder.HasKey(pa => pa.Id);

            builder.Property(pa => pa.ActionName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasIndex(pa => pa.ActionName);
        }
    }
}