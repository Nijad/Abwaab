using Abwaab.Application.DTOs.Profile.NotificationWayUnsubscription;
using FluentValidation;

namespace Abwaab.Server.Validations.Profile
{
    public class NotificationWayUnsubscriptionCommandValidation : AbstractValidator<NotificationWaySubsciptionCommand>
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
