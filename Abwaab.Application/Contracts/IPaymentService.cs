using Abwaab.Application.Common.Enums;
using Abwaab.Domain.Entities.PaymentEntities;

namespace Abwaab.Application.Contracts
{
    public interface IPaymentService
    {
        Task ConfirmPaymentAsync(Payment payment, string errorTitle);
        Task<Payment?> FindPaymentByPaymentCodeAsync(string paymentCode);
        Task<PaymentState> FindPaymentSateBySateNameAsync(PaymentStatesEnum stateName, string errorTitle);
        Task<Payment?> FindPendingUserPlanPaymentAsync(Guid userPlanId, string errorTitle);
        Task UpdatePaymentAsync(Payment userPlanPendingPayment);
    }
}
