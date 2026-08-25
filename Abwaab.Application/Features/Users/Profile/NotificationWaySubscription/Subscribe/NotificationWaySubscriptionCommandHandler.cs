using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Auth;
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
        private readonly IUserService _userService;
        private readonly string errorTitle = ErrorTitle.NotificationWaySubscription;

        public NotificationWaySubscriptionCommandHandler(
            IProfileService profileService,
            UserManager<ApplicationUser> userManager,
            IUserService userService)
        {
            _profileService = profileService;
            _userManager = userManager;
            _userService = userService;
        }

        public async Task<NotificationWaySubscriptionResponse> Handle(NotificationWaySubscriptionCommand request, CancellationToken cancellationToken)
        {
            //check if user exist

            string username = _userService.FindUserNameByContext(errorTitle);
            ApplicationUser? user = await _userManager.FindByNameAsync(username);
            
            if (user == null)
                throw new UserNotFoundException(username, errorTitle);

            NotificationWaySubscriptionResponse response = await _profileService.SubscribeNotificationWayCommandAsync(user, request.NotifiactionWayId, errorTitle);

            return response;
        }
    }
}
