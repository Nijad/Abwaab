using Abwaab.Domain.Entities.MediaEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abwaab.Infrastructure.Presistence.Configureations
{
    public class MediaTypeConfiguration : IEntityTypeConfiguration<MediaType>
    {
        public void Configure(EntityTypeBuilder<MediaType> builder)
        {
            builder.ToTable("MediaTypes");

            builder.HasKey(mt => mt.Id);

            builder.Property(mt => mt.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(mt => mt.Description)
                   .HasMaxLength(1000);

            //builder.HasMany(mt => mt.MediaList)
            //       .WithOne(m => m.MediaType)
            //       .HasForeignKey(m => m.MediaTypeId)
            //       .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(mt => mt.Name).IsUnique();
        }
    }
}