using Abwaab.Domain.Entities.UserEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abwaab.Infrastructure.Presistence.Configureations
{
    public class OTPConfiguration : IEntityTypeConfiguration<OTP>
    {
        public void Configure(EntityTypeBuilder<OTP> builder)
        {
            builder.ToTable("OTPs");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Code)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(o => o.ExpiredAt)
                   .IsRequired();

            builder.Property(o => o.IsUsed)
                   .IsRequired();

            builder.HasOne(o => o.User)
                   .WithMany(u => u.OTPs)
                   .HasForeignKey(o => o.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(o => o.UserId);
            builder.HasIndex(o => o.Code);
            builder.HasIndex(o => o.ExpiredAt);
        }
    }
}