using FluentValidation;

namespace Abwaab.Application.Features.Payments.Confirm
{
    public class ConfirmPaymentValidation : AbstractValidator<ConfirmPaymentCommand>
    {
        public ConfirmPaymentValidation()
        {
            RuleFor(x => x.paymentCode)
                .NotEmpty().WithMessage("Payment code is required");
        }
    }
}
