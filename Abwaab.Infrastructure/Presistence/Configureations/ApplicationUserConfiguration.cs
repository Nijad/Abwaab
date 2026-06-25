using Abwaab.Domain.Entities.UserEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abwaab.Infrastructure.Presistence.Configureations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            // AspNetUsers is the default Identity table name; adjust if you use a custom name.
            builder.ToTable("AspNetUsers");

            builder.Property(u => u.FirstName)
                   .HasMaxLength(100);

            builder.Property(u => u.LastName)
                   .HasMaxLength(100);

            builder.Property(u => u.RefreshToken)
                   .HasMaxLength(4000);

            builder.Property(u => u.RefreshTokenExpiryTime)
                   .IsRequired(false);

            builder.HasIndex(u => u.FirstName);
            builder.HasIndex(u => u.LastName);
        }
    }
}