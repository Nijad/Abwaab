namespace Abwaab.Domain.Entities.PaymentEntities
{
    public class ServiceType : BaseEntity
    {
        public string ServiceName { get; set; } = null!;
        public List<Payment>? Payments { get; set; }
    }
}
