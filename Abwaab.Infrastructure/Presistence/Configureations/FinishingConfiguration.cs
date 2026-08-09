using Abwaab.Domain.Entities.PropertyEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abwaab.Infrastructure.Presistence.Configureations
{
    public class FinishingConfiguration : IEntityTypeConfiguration<Finishing>
    {
        public void Configure(EntityTypeBuilder<Finishing> builder)
        {
            builder.ToTable("Finishings");

            builder.HasKey(f => f.Id);

            builder.Property("FinishingName")
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasIndex("FinishingName").IsUnique();
        }
    }
}