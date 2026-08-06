using Abwaab.Domain.Entities.PaymentEntities;
using Abwaab.Domain.Entities.PropertyEntities;

namespace Abwaab.Domain.Entities.UserEntities
{
    public class UserPlan: BaseEntity
    {
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public Guid PlanId { get; set; }
        public Plan Plan { get; set; } = null!;
        public DateOnly SubscriptionDate { get; set; }
        public Guid UserPlanStateId { get; set; }
        public UserPlanStatus? UserPlanStatus { get; set; }
        public List<Payment>? Payments { get; set; }
        public List<Property>? Properties { get; set; }
    }
}
