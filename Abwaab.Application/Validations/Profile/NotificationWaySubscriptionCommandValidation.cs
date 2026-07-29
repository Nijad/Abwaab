using Abwaab.Application.DTOs.Profile.NotificationWaySubscription;
using FluentValidation;

namespace Abwaab.Application.Validations.Profile
{
    public class NotificationWaySubscriptionCommandValidation : AbstractValidator<NotificationWaySubscriptionCommand>
    {
        public NotificationWaySubscriptionCommandValidation()
        {
            RuleFor(nt => nt.UserId)
                .NotEmpty().WithMessage("User Id is required");
            
            RuleFor(nt => nt.NotifiactionWayId)
                .NotEmpty().WithMessage("Notification Way Id is required");
        }
    }
}
