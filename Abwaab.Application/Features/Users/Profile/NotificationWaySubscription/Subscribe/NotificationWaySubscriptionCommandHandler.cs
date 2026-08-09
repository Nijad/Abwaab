using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Contracts;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Abwaab.Application.Features.Users.Profile.NotificationWaySubscription.Subscribe
{
    public class NotificationWaySubscriptionCommandHandler : IRequestHandler<NotificationWaySubscriptionCommand, NotificationWaySubscriptionResponse>
    {
        private readonly IProfileService _profileService;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationWaySubscriptionCommandHandler(
            IProfileService profileService, 
            UserManager<ApplicationUser> userManager)
        {
            _profileService = profileService;
            _userManager = userManager;
        }

        public async Task<NotificationWaySubscriptionResponse> Handle(NotificationWaySubscriptionCommand request, CancellationToken cancellationToken)
        {
            //check if user exist
            ApplicationUser? user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
                throw new NotFoundException("User", nameof(request.UserId), request.UserId.ToString());

            NotificationWaySubscriptionResponse response = await _profileService.SubscribeNotificationWayCommandAsync(user, request.NotifiactionWayId);

            return response;
        }
    }
}
