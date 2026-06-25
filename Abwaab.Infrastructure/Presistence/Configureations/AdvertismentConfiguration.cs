using Abwaab.Domain.Entities.MediaEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abwaab.Infrastructure.Presistence.Configureations
{
    public class AdvertismentConfiguration : IEntityTypeConfiguration<Advertisment>
    {
        public void Configure(EntityTypeBuilder<Advertisment> builder)
        {
            builder.ToTable("Advertisments");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Url)
                   .IsRequired()
                   .HasMaxLength(2000);

            builder.Property(a => a.Title)
                   .HasMaxLength(500);

            builder.Property(a => a.Description)
                   .HasMaxLength(2000);

            // store DateOnly as SQL date
            builder.Property(a => a.StartDisplayDate)
                   .HasColumnType("date")
                   .IsRequired();

            builder.Property(a => a.EndDisplayDate)
                   .HasColumnType("date")
                   .IsRequired();

            builder.HasIndex(a => a.StartDisplayDate);
            builder.HasIndex(a => a.EndDisplayDate);
        }
    }
}