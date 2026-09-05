using Abwaab.Domain.Entities.UserEntities;

namespace Abwaab.Domain.Entities.PaymentEntities
{
    public class Payment : BaseEntity
    {
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public string PaymentCode { get; set; } = null!;
        public DateTime PayedAt { get; set; }
        public PaymentState PaymentState { get; set; } = null!;
        public Guid PaymentStateId { get; set; }
        public ServiceType ServiceType { get; set; } = null!;
        public Guid ServiceTypeId { get; set; }
        public Guid? UserPlandId { get; set; }
        public UserPlan? UserPlan { get; set; }
    }
}
