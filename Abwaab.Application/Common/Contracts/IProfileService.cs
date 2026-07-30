using Abwaab.Application.Features.Users.Profile.Email.Cancel;
using Abwaab.Application.Features.Users.Profile.Email.Confirm;
using Abwaab.Application.Features.Users.Profile.Email.InitiateChange;
using Abwaab.Application.Features.Users.Profile.NotificationWaySubscription.Subscribe;
using Abwaab.Application.Features.Users.Profile.NotificationWaySubscription.Unsubscribe;
using Abwaab.Application.Features.Users.Profile.Password.Change;
using Abwaab.Application.Features.Users.Profile.Password.Forgot;
using Abwaab.Application.Features.Users.Profile.Phone.Cancel;
using Abwaab.Application.Features.Users.Profile.Phone.Confirm;
using Abwaab.Application.Features.Users.Profile.Phone.InitiateChange;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Domain.Enums;

namespace Abwaab.Application.Common.Contracts
{
    public interface IProfileService
    {
        Task<NotificationWaySubscriptionResponse> SubscribeNotificationWayCommandAsync(NotificationWaySubscriptionCommand request);
        Task<NotificationWayUnsubscriptionResponse> UnsubscribeNotificationWayCommandAsync(NotificationWaySubsciptionCommand request);
        Task<ForgotPasswordResponse> ForgotPasswordCommandAsyn(ForgotPasswordDTO request);
        Task<ChangePasswordResponse> ChangePasswordCommandAsync(ChangePasswordDTO request);
        Task<bool> SubscribeNotificationWayCommandAsync(ApplicationUser user, NotificationWayEnum notificationWayType);
        Task<InitiateEmailChangeResponse> InitiatieEmailChangeCommandAsync(InitiateEmailChangeCommand request);
        Task<ConfirmEmailChangeResponse> ConfirmEmailChangeCommandAsync(ConfirmEmailChangeCommand request);
        Task<InitiatePhoneNoChangeResponse> InitiatePhoneNoChangeCommandAsync(InitiatePhoneNoChangeCommand request);
        Task<ConfirmPhoneNoChangeResponse> ConfirmPhoneNoChangeCommandAsync(ConfirmPhoneNoChangeCommand request);
        Task<CancelEmailChangeResponse> CancelEmailChangeCommandAsync(CancelEmailChangeCommand request);
        Task<CancelPhoneChangeResponse> CancelPhoneChangeCommandAsync(CancelPhoneChangeCommand request);
    }
}
