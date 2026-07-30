using Abwaab.Application.Common.Contracts;
using MediatR;

namespace Abwaab.Application.Features.Users.Profile.NotificationWaySubscription.Subscribe
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
