using FluentValidation;

namespace Abwaab.Application.Features.Users.Profile.NotificationWaySubscription.Subscribe
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
