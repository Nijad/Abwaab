using Abwaab.Application.Common.Contracts;
using Abwaab.Application.DTOs.Profile.NotificationWaySubscription;
using MediatR;

namespace Abwaab.Server.Handlers.ProfileHandlers
{
    public class NotificationWaySubscriptionCommandHandler : IRequestHandler<NotificationWaySubscriptionCommand, NotificationWaySubscriptionResponse>
    {
        IProfileService _profileService;
        public NotificationWaySubscriptionCommandHandler(IProfileService profileService)
        {
            _profileService = profileService;
        }

        public async Task<NotificationWaySubscriptionResponse> Handle(NotificationWaySubscriptionCommand request, CancellationToken cancellationToken)
        {
            NotificationWaySubscriptionResponse response = await _profileService.SubscribeNotificationWayCommandAsync(request);
            return response;
        }
    }
}
