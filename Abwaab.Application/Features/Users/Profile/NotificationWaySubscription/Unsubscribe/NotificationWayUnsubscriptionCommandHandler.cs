using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Contracts;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Abwaab.Application.Features.Users.Profile.NotificationWaySubscription.Unsubscribe
{
    public class NotificationWayUnsubscriptionCommandHandler : IRequestHandler<NotificationWayUnsubsciptionCommand, NotificationWayUnsubscriptionResponse>
    {
        private readonly IProfileService _profileService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        private readonly string errorTitle = ErrorTitle.NotificationWayUnsubscription;

        public NotificationWayUnsubscriptionCommandHandler(
            IProfileService profileService, UserManager<ApplicationUser> userManager, IUserService userService)
        {
            _profileService = profileService;
            _userManager = userManager;
            _userService = userService;
        }

        public async Task<NotificationWayUnsubscriptionResponse> Handle(NotificationWayUnsubsciptionCommand request, CancellationToken cancellationToken)
        {
            //check if user exist
            string username = _userService.FindUserNameByContext();
            ApplicationUser? user = await _userManager.FindByNameAsync(username);
            if (user == null)
                throw new UserNotFoundException(username, errorTitle);

            NotificationWayUnsubscriptionResponse response = await _profileService.UnsubscribeNotificationWayCommandAsync(user, request.NotifiactionWayId, errorTitle);
            return response;
        }
    }
}
