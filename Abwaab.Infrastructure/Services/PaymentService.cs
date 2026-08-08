using Abwaab.Application.Common.Enums;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Payments;
using Abwaab.Application.Contracts;
using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.PaymentEntities;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Infrastructure.Presistence.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Abwaab.Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPlanRepository _planRepository;
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string actionBy;

        public PaymentService(
            AppDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IPlanRepository planRepository)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            actionBy = _httpContextAccessor!.HttpContext!.User!.Identity!.Name!;
            _planRepository = planRepository;
        }

        public async Task ConfirmPaymentAsync(Payment payment)
        {
            Guid completedStateId = await GetPaymentStateIdByNameAsyn(PaymentStatesEnum.Completed);

            payment.PaymentStateId = completedStateId;
            payment.LastModifiedAt = DateTime.Now;
            payment.LastModifiedBy = actionBy;

            if (payment.ServiceType.ServiceName == ServiceTypesEnum.Plan_Subscription.ToString().Replace("_", " "))
            {
                UserPlanStatus? activeUserPlanState = await _planRepository.GetUserPlanStatusByNameAsync(UserPlanStatesEnum.Active.ToString());
                
                payment.UserPlan!.UserPlanStateId = activeUserPlanState!.Id;
                payment.UserPlan.UserPlanStatus = activeUserPlanState;
            }
            else if (payment.ServiceType.ServiceName == ServiceTypesEnum.Advertisment.ToString())
                throw new NotImplementedServiceTypeException(ServiceTypesEnum.Plan_Subscription.ToString().Replace("_", " "));
            else
                throw new NotImplementedServiceTypeException(ServiceTypesEnum.Plan_Subscription.ToString().Replace("_", " "));

            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }

        private async Task<Guid> GetPaymentStateIdByNameAsyn(PaymentStatesEnum state)
        {
            string stateName = state.ToString();
            PaymentState? paymentState = await _context.PaymentStates.Where(x => x.StateName == stateName).FirstOrDefaultAsync();

            if (paymentState == null)
                throw new NotFoundException(nameof(PaymentState), nameof(paymentState.StateName), stateName);

            return paymentState.Id;
        }

        public async Task<Payment?> FindPaymentByPaymentCodeAsync(string paymentCode)
        {
            return await _context.Payments
                .Include(x=>x.ServiceType)
                .Include(x=>x.UserPlan)
                .Include(x=>x.Advertisment)
                .Where(x => x.PaymentCode == paymentCode)
                .FirstOrDefaultAsync();
        }
    }
}
