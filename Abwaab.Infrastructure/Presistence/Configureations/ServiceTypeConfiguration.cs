using Abwaab.Domain.Entities.PaymentEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abwaab.Infrastructure.Presistence.Configureations
{
    public class ServiceTypeConfiguration : IEntityTypeConfiguration<ServiceType>
    {
        public void Configure(EntityTypeBuilder<ServiceType> builder)
        {
            builder.ToTable("ServiceTypes");

            builder.HasKey(st => st.Id);

            builder.Property(st => st.ServiceName)
                   .IsRequired()
                   .HasMaxLength(200);

            //builder.HasMany(st => st.Payments)
            //       .WithOne(p => p.ServiceType)
            //       .HasForeignKey(p => p.ServiceTypeId)
            //       .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(st => st.ServiceName).IsUnique();
        }
    }
}