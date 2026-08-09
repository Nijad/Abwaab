using Abwaab.Domain.Entities.PropertyEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abwaab.Infrastructure.Presistence.Configureations
{
    public class PropertyStateConfiguration : IEntityTypeConfiguration<PropertyState>
    {
        public void Configure(EntityTypeBuilder<PropertyState> builder)
        {
            builder.ToTable("PropertyStates");

            builder.HasKey(ps => ps.Id);

            // match AppointmentState naming pattern if your PropertyState uses StateName
            builder.Property("StateName")
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasIndex("StateName").IsUnique();
        }
    }
}