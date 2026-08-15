using FluentValidation;

namespace Abwaab.Application.Features.Users.Profile.NotificationWaySubscription.Unsubscribe
{
    public class NotificationWayUnsubscriptionCommandValidation : AbstractValidator<NotificationWayUnsubsciptionCommand>
    {
        public NotificationWayUnsubscriptionCommandValidation()
        {
            RuleFor(nt => nt.UserId)
                .NotEmpty().WithMessage("رقم تعريف المستخدم مطلوب");

            RuleFor(nt => nt.NotifiactionWayId)
                .NotEmpty().WithMessage("طريقة الإشعار مطلوبة");
        }
    }
}
