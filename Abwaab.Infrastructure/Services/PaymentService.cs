using Abwaab.Application.Common.Enums;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Contracts;
using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.PaymentEntities;
using Abwaab.Infrastructure.Presistence.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Abwaab.Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPaymentRepository _paymentRepository;
        private readonly string actionBy;

        public PaymentService(
            AppDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IPaymentRepository paymentRepository)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            actionBy = _httpContextAccessor!.HttpContext!.User!.Identity!.Name!;
            _paymentRepository = paymentRepository;
        }

        public async Task ConfirmPaymentAsync(Payment payment, string errorTitle)
        {
            Guid completedStateId = await GetPaymentStateIdByNameAsyn(PaymentStatesEnum.Completed, errorTitle);

            payment.PaymentStateId = completedStateId;
            payment.LastModifiedAt = DateTime.Now;
            payment.LastModifiedBy = actionBy;
            
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }

        private async Task<Guid> GetPaymentStateIdByNameAsyn(PaymentStatesEnum state, string errorTitle)
        {
            string stateName = state.ToString();
            PaymentState? paymentState = await _context.PaymentStates.Where(x => x.StateName == stateName).FirstOrDefaultAsync();

            if (paymentState == null)
                throw new NotFoundException(nameof(PaymentState), nameof(paymentState.StateName), stateName, errorTitle);

            return paymentState.Id;
        }

        public async Task<Payment?> FindPaymentByPaymentCodeAsync(string paymentCode)
        {
            return await _context.Payments
                .Include(x => x.ServiceType)
                .Include(x => x.UserPlan)
                .Where(x => x.PaymentCode == paymentCode)
                .FirstOrDefaultAsync();
        }

        public async Task<PaymentState> FindPaymentSateBySateNameAsync(PaymentStatesEnum stateName, string errorTitle)
        {
            PaymentState? state = await _context.PaymentStates.Where(x => x.StateName == stateName.ToString()).FirstOrDefaultAsync();

            if (state == null)
                throw new NotFoundException(nameof(PaymentState), nameof(state.StateName), stateName.ToString(), errorTitle);

            return state;
        }

        public async Task<Payment?> FindPendingUserPlanPaymentAsync(Guid userPlanId, string errorTitle)
        {
            PaymentState pendingPaymentState = await FindPaymentSateBySateNameAsync(PaymentStatesEnum.Pending, errorTitle);

            return await _context.Payments.Where(x => x.UserPlandId == userPlanId && x.PaymentStateId == pendingPaymentState.Id).FirstOrDefaultAsync();
        }

        public async Task UpdatePaymentAsync(Payment userPlanPendingPayment)
        {
            _context.Payments.Update(userPlanPendingPayment);
            await _context.SaveChangesAsync();
        }

        public async Task AddPaymentAsync(Payment payment)
        {
            payment.CreatedAt = DateTime.Now;
            payment.CreatedBy = actionBy;
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
        }

        public async Task<ServiceType> FindServicTypeByNameAsync(ServiceTypesEnum plan_Subscription, string errorTitle)
        {
            ServiceType? serviceType = await _paymentRepository.FindServiceTypeByName(plan_Subscription);

            if (serviceType == null)
                throw new NotFoundException(
                    nameof(ServiceType),
                    nameof(serviceType.ServiceName),
                    plan_Subscription.ToString(),
                    errorTitle);

            return serviceType;
        }
    }
}
