using MediatR;

namespace Abwaab.Application.Features.Payments.Confirm
{
    public class ConfirmPaymentCommand : IRequest<ConfirmPaymentResponse>
    {
        public string paymentCode { get; set; } = string.Empty;
    }
}
