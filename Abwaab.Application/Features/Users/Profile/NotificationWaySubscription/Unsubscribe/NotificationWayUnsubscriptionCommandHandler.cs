using Abwaab.Application.Common.Contracts;
using MediatR;

namespace Abwaab.Application.Features.Users.Profile.NotificationWaySubscription.Unsubscribe
{
    public class NotificationWayUnsubscriptionCommandHandler : IRequestHandler<NotificationWaySubsciptionCommand, NotificationWayUnsubscriptionResponse>
    {
        IProfileService _profileService;
        public NotificationWayUnsubscriptionCommandHandler(
            IProfileService profileService)
        {
            _profileService = profileService;
        }

        public async Task<NotificationWayUnsubscriptionResponse> Handle(NotificationWaySubsciptionCommand request, CancellationToken cancellationToken)
        {
            NotificationWayUnsubscriptionResponse response = await _profileService.UnsubscribeNotificationWayCommandAsync(request);
            return response;
        }
    }
}
