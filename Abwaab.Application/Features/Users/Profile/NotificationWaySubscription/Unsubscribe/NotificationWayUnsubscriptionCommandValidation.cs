using FluentValidation;

namespace Abwaab.Application.Features.Users.Profile.NotificationWaySubscription.Unsubscribe
{
    public class NotificationWayUnsubscriptionCommandValidation : AbstractValidator<NotificationWayUnsubsciptionCommand>
    {
        public NotificationWayUnsubscriptionCommandValidation()
        {
            RuleFor(nt => nt.UserId)
                .NotEmpty().WithMessage("User Id is required");
            
            RuleFor(nt => nt.NotifiactionWayId)
                .NotEmpty().WithMessage("Notification Way Id is required");
        }
    }
}
