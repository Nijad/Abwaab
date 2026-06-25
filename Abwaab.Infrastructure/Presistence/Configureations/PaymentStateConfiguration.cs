using Abwaab.Domain.Entities.PaymentEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abwaab.Infrastructure.Presistence.Configureations
{
    public class PaymentStateConfiguration : IEntityTypeConfiguration<PaymentState>
    {
        public void Configure(EntityTypeBuilder<PaymentState> builder)
        {
            builder.ToTable("PaymentStates");

            builder.HasKey(ps => ps.Id);

            builder.Property(ps => ps.StateName)
                   .IsRequired()
                   .HasMaxLength(200);

            // Payments collection navigation (if present)
            //builder.HasMany(ps => ps.Payments)
            //       .WithOne(p => p.PaymentState)
            //       .HasForeignKey(p => p.PaymentStateId)
            //       .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(ps => ps.StateName).IsUnique();
        }
    }
}