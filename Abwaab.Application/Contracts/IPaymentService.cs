using Abwaab.Application.Common.Enums;
using Abwaab.Domain.Entities.PaymentEntities;

namespace Abwaab.Application.Contracts
{
    public interface IPaymentService
    {
        Task AddPaymentAsync(Payment payment);
        Task ConfirmPaymentAsync(Payment payment, string errorTitle);
        Task<Payment?> FindPaymentByPaymentCodeAsync(string paymentCode);
        Task<PaymentState> FindPaymentSateBySateNameAsync(PaymentStatesEnum stateName, string errorTitle);
        Task<Payment?> FindPendingUserPlanPaymentAsync(Guid userPlanId, string errorTitle);
        Task<ServiceType> FindServicTypeByNameAsync(ServiceTypesEnum plan_Subscription, string errorTitle);
        Task UpdatePaymentAsync(Payment userPlanPendingPayment);
    }
}
