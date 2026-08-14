using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Enums;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Payments;
using Abwaab.Application.Contracts;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.PaymentEntities;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;

namespace Abwaab.Application.Features.Payments.Confirm
{
    public class ConfirmPaymentHandler : IRequestHandler<ConfirmPaymentCommand, ConfirmPaymentResponse>
    {
        private readonly ITransactionManager _transactionManager;
        private readonly IPaymentService _paymentService;
        private readonly IPlanService _planService;
        private readonly IUserPlanStateService _userPlanStateService;
        private readonly string errorTitle = ErrorTitle.ConfirmPayment;

        public ConfirmPaymentHandler(IPaymentService paymentService, ITransactionManager transactionManager, IPlanService planService, IUserPlanStateService userPlanStateService)
        {
            _paymentService = paymentService;
            _transactionManager = transactionManager;
            _planService = planService;
            _userPlanStateService = userPlanStateService;
        }

        public async Task<ConfirmPaymentResponse> Handle(ConfirmPaymentCommand request, CancellationToken cancellationToken)
        {
            await _transactionManager.BeginTransactionAsync(cancellationToken);
            try
            {
                // 1. CONFIRMATION PAYMENT
                // get payemnet by payment code
                Payment? payment = await _paymentService.FindPaymentByPaymentCodeAsync(request.paymentCode);

                if (payment == null)
                    throw new NotFoundException(
                        entity: nameof(Payment), 
                        property: nameof(request.paymentCode), 
                        value: request.paymentCode, 
                        title: errorTitle);

                PaymentState pendingPayment = await _paymentService.FindPaymentSateBySateNameAsync(PaymentStatesEnum.Pending, errorTitle);

                if (payment.PaymentStateId != pendingPayment.Id)
                    throw new NotValidPaymentCodeException(errorTitle);

                //change its state to paid
                await _paymentService.ConfirmPaymentAsync(payment, errorTitle);


                // 2. ACTIVATION SERVICE


                if (payment.ServiceType.ServiceName == ServiceTypesEnum.Plan_Subscription.ToString().Replace("_", " "))
                {
                    UserPlanStatus? activeUserPlanState = await _userPlanStateService.GetActiveUserPlanStatus(errorTitle);

                    UserPlanStatus? workingUserPlanState = await _userPlanStateService.GetWorkingUserPlanStatus(errorTitle);

                    //find active plan if exist and change status to working
                    UserPlan activePlan = await _planService.FindUserActivePlanAsync(payment.UserPlan!.UserId, activeUserPlanState.Id, errorTitle);

                    if (activePlan != null)
                    {
                        activePlan.UserPlanStatus = workingUserPlanState;
                        activePlan.UserPlanStateId = workingUserPlanState.Id;
                        await _planService.UpdateUserPlan(activePlan);
                    }
                    payment.UserPlan.UserPlanStateId = activeUserPlanState!.Id;
                    payment.UserPlan.UserPlanStatus = activeUserPlanState;
                    await _planService.UpdateUserPlan(payment.UserPlan);
                }
                else if (payment.ServiceType.ServiceName == ServiceTypesEnum.Advertisment.ToString())
                    throw new NotImplementedServiceTypeException(ServiceTypesEnum.Advertisment.ToString(), errorTitle);
                else
                    throw new NotImplementedServiceTypeException(payment.ServiceType.ServiceName, errorTitle);

                await _transactionManager.CommitTransactionAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await _transactionManager.RollbackTransactionAsync(cancellationToken);
                throw;
            }
            return new() { Success = true, Message = $"Payment Confirmed, and {ServiceTypesEnum.Plan_Subscription.ToString().Replace("_", " ")} activated successfully" };
        }
    }
}
