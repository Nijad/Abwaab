using Abwaab.Application.Common.Enums;
using Abwaab.Domain.Entities.PaymentEntities;

namespace Abwaab.Application.Contracts
{
    public interface IPaymentService
    {
        Task ConfirmPaymentAsync(Payment payment);
        Task<Payment?> FindPaymentByPaymentCodeAsync(string paymentCode);
        Task<PaymentState> FindPaymentSateBySateNameAsync(PaymentStatesEnum stateName);
        Task<Payment?> FindPendingUserPlanPaymentAsync(Guid userPlanId);
        Task UpdatePaymentAsync(Payment userPlanPendingPayment);
    }
}
