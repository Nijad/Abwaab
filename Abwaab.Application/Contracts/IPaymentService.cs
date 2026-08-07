using Abwaab.Domain.Entities.PaymentEntities;

namespace Abwaab.Application.Contracts
{
    public interface IPaymentService
    {
        Task ConfirmPaymentAsync(Payment payment);
        Task<Payment?> FindPaymentByPaymentCodeAsync(string paymentCode);
    }
}
