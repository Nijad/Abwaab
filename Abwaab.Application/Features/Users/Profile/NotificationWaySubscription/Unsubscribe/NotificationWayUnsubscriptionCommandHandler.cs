using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Contracts;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Abwaab.Application.Features.Users.Profile.NotificationWaySubscription.Unsubscribe
{
    public class NotificationWayUnsubscriptionCommandHandler : IRequestHandler<NotificationWayUnsubsciptionCommand, NotificationWayUnsubscriptionResponse>
    {
        IProfileService _profileService;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationWayUnsubscriptionCommandHandler(
            IProfileService profileService, UserManager<ApplicationUser> userManager)
        {
            _profileService = profileService;
            _userManager = userManager;
        }

        public async Task<NotificationWayUnsubscriptionResponse> Handle(NotificationWayUnsubsciptionCommand request, CancellationToken cancellationToken)
        {
            //check if user exist
            ApplicationUser? user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
                throw new NotFoundException("User", nameof(request.UserId), request.UserId.ToString());

            NotificationWayUnsubscriptionResponse response = await _profileService.UnsubscribeNotificationWayCommandAsync(user, request.NotifiactionWayId);
            return response;
        }
    }
}
