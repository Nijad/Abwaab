using Abwaab.Domain.Entities.PaymentEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abwaab.Infrastructure.Presistence.Configureations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Amount)
                   .HasPrecision(18, 2)
                   .IsRequired();

            builder.Property(p => p.Description)
                   .HasMaxLength(4000);

            builder.Property(p => p.PaymentCode)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(p => p.PayedAt)
                   .IsRequired();

            // Required lookup relationships
            builder.HasOne(p => p.PaymentState)
                   .WithMany(s => s.Payments)
                   .HasForeignKey(p => p.PaymentStateId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.ServiceType)
                   .WithMany(st => st.Payments)
                   .HasForeignKey(p => p.ServiceTypeId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Optional relationships - keep payments if related entity is removed
            builder.HasOne(p => p.UserPlan)
                   .WithMany(u => u.Payments)
                   .HasForeignKey(p => p.UserPlandId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(p => p.Advertisment)
                   .WithMany(a => a.Payments)
                   .HasForeignKey(p => p.AdvertismentId)
                   .OnDelete(DeleteBehavior.SetNull);

            // Indexes
            builder.HasIndex(p => p.PaymentStateId);
            builder.HasIndex(p => p.ServiceTypeId);
            builder.HasIndex(p => p.UserPlandId);
            builder.HasIndex(p => p.AdvertismentId);
            builder.HasIndex(p => p.PaymentCode);
            builder.HasIndex(p => p.PayedAt);
        }
    }
}