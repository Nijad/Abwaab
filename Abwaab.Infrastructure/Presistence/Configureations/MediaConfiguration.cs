using Abwaab.Domain.Entities.MediaEntities;
using Abwaab.Domain.Entities.PropertyEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abwaab.Infrastructure.Presistence.Configureations
{
    public class MediaConfiguration : IEntityTypeConfiguration<Media>
    {
        public void Configure(EntityTypeBuilder<Media> builder)
        {
            builder.ToTable("Media");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.FilePath)
                   .IsRequired()
                   .HasMaxLength(200);


            builder.HasOne(m => m.Property)
                   .WithMany(p => p.MediaList)
                   .HasForeignKey(m=>m.PropertyId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.MediaType)
                .WithMany(t => t.MediaList)
                .HasForeignKey(m => m.MediaTypeId)
                .OnDelete(DeleteBehavior.Cascade);
                

            builder.HasIndex(m => m.PropertyId);
        }
    }
}