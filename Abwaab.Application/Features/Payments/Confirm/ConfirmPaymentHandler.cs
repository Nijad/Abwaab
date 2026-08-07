using Abwaab.Application.Common.Enums;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Contracts;
using Abwaab.Domain.Entities.PaymentEntities;
using MediatR;

namespace Abwaab.Application.Features.Payments.Confirm
{
    public class ConfirmPaymentHandler : IRequestHandler<ConfirmPaymentCommand, ConfirmPaymentResponse>
    {
        private readonly IPaymentService _paymentService;
        private readonly IPlanService _planService;

        public ConfirmPaymentHandler(
            IPaymentService paymentService, 
            IPlanService planService)
        {
            _paymentService = paymentService;
            _planService = planService;
        }

        public async Task<ConfirmPaymentResponse> Handle(ConfirmPaymentCommand request, CancellationToken cancellationToken)
        {
            // get payemnet by payment code
            Payment? payment = await _paymentService.FindPaymentByPaymentCodeAsync(request.paymentCode);

            if (payment == null)
                throw new NotFoundException(nameof(Payment), nameof(request.paymentCode), request.paymentCode);

            //change its state to paid
            await _paymentService.ConfirmPaymentAsync(payment);

            return new() { Success = true, Message = $"Payment Confirmed, and {ServiceTypesEnum.Plan_Subscription.ToString().Replace("_", " ")} activated successfully" };
        }
    }
}
