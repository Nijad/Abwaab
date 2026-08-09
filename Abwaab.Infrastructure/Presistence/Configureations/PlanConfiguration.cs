using Abwaab.Domain.Entities.UserEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abwaab.Infrastructure.Presistence.Configureations
{
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.ToTable("Plans");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(p => p.Price)
                   .HasPrecision(18, 2);

            builder.Property(p => p.DurationInDays)
                   .IsRequired();

            builder.Property(p => p.TempDurationInDays)
                   .IsRequired();

            builder.Property(p => p.MaxPropertiesCountAtSameTime)
                   .IsRequired();

            builder.Property(p => p.MaxStardPropertiesCountAtSameTime)
                   .IsRequired();

            builder.Property(p => p.MaxImagesCount)
                   .IsRequired();

            builder.Property(p => p.MaxVideosCount)
                   .IsRequired();

            builder.Property(p => p.IsDisabled)
                   .IsRequired()
                   .HasDefaultValue(false);

            // Map DateOnly to SQL date
            builder.Property(p => p.StartDate)
                   .HasColumnType("date");

            builder.Property(p => p.ExpieryDate)
                   .HasColumnType("date");

            builder.Property(p => p.DefaultPlan)
                   .IsRequired(false)
                   .HasDefaultValue(false);

            // Indexes
            builder.HasIndex(p => p.Name);
            builder.HasIndex(p => p.IsDisabled);
        }
    }
}