using Abwaab.Domain.Entities.AppointmentEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abwaab.Infrastructure.Presistence.Configureations
{
    public class AppointmentActionConfiguration : IEntityTypeConfiguration<AppointmentAction>
    {
        public void Configure(EntityTypeBuilder<AppointmentAction> builder)
        {
            builder.ToTable("AppointmentActions");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.ActionName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasIndex(a => a.ActionName);
        }
    }
}