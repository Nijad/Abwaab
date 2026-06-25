namespace Abwaab.Domain.Entities.PaymentEntities
{
    public class PaymentState : BaseEntity
    {
        public string StateName { get; set; } = null!;
        public List<Payment>? Payments { get; set; }
    }
}
