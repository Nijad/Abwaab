using FluentValidation;

namespace Abwaab.Application.Features.Users.Profile.NotificationWaySubscription.Subscribe
{
    public class NotificationWaySubscriptionCommandValidation : AbstractValidator<NotificationWaySubscriptionCommand>
    {
        public NotificationWaySubscriptionCommandValidation()
        {
            RuleFor(nt => nt.UserId)
                .NotEmpty().WithMessage("رقم تعريف المستخدم مطلوب");
            
            RuleFor(nt => nt.NotifiactionWayId)
                .NotEmpty().WithMessage("طريقة الإشعار مطلوبة");
        }
    }
}
