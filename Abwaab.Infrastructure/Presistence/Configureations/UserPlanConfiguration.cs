using Abwaab.Domain.Entities.UserEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abwaab.Infrastructure.Presistence.Configureations
{
    public class UserPlanConfiguration : IEntityTypeConfiguration<UserPlan>
    {
        public void Configure(EntityTypeBuilder<UserPlan> builder)
        {
            builder.ToTable("UserPlans");

            builder.HasKey(p => p.Id);
            
            builder.Property(p => p.SubscriptionDate)
                   .IsRequired(true)
                   .HasColumnType("date");

            builder.Property(p => p.IsActive)
                .IsRequired(true)
                .HasDefaultValue(false)
                .HasColumnType("bit");

            // Relationships
            builder.HasOne(p => p.User)
                   .WithMany(u => u.UserPlans)
                   .HasForeignKey(p => p.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasOne(p => p.Plan)
                   .WithMany(u => u.UserPlans)
                   .HasForeignKey(p => p.PlanId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(p => p.UserId);
            builder.HasIndex(p => p.PlanId);
        }
    }
}
