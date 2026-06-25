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

            // Relationship -> Property (assumes Property.PropertyStateId exists)
            //builder.HasMany<Property>(ps => new List<Property>())
            //       .WithOne()
            //       .HasForeignKey("PropertyStateId")
            //       .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex("StateName").IsUnique();
        }
    }
}