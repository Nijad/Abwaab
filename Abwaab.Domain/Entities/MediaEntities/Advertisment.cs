using Abwaab.Domain.Entities.PaymentEntities;

namespace Abwaab.Domain.Entities.MediaEntities
{
    public class Advertisment : BaseEntity
    {
        public string Url { get; set; } = null!;
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateOnly StartDisplayDate { get; set; }
        public DateOnly EndDisplayDate { get; set; }
        public List<Payment>? Payments { get; set; }
    }
}