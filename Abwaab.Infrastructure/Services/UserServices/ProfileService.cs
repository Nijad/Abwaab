using Abwaab.Application.Common.Contracts;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Interfaces;
using Abwaab.Application.DTOs.Profile.NotificationWaySubscription;
using Abwaab.Application.DTOs.Profile.NotificationWayUnsubscription;
using Abwaab.Domain.Entities.NotificationEntities;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Abwaab.Infrastructure.Services.UserServices
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly IUserService _userService;
        private readonly INotificationWayRepository _notificationWayRepository;
        public ProfileService(
            UserManager<ApplicationUser> userManager,
            //IUserService userService,
            INotificationWayRepository notificationWayRepository)
        {
            _userManager = userManager;
            //_userService = userService;
            _notificationWayRepository = notificationWayRepository;
        }
        public async Task<NotificationWaySubscriptionResponse> SubscribeNotificationWayCommandAsync(NotificationWaySubscriptionCommand request)
        {
            //check if user exist
            ApplicationUser? user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
                throw new NotFoundException("User", nameof(request.UserId), request.UserId.ToString());

            //check if notification way exist
            NotificationWay? notificationWay = await _notificationWayRepository.GetNotificationWayByIdAsync(request.NotifiactionWayId);
            if (notificationWay == null)
                throw new NotFoundException(nameof(NotificationWay), nameof(request.NotifiactionWayId), request.NotifiactionWayId.ToString());

            //check if user had already subscribe
            UserNotificationSubscription? userSubscription = await _notificationWayRepository.GetUserSubscriptionAsync(request.UserId, request.NotifiactionWayId);

            if (userSubscription != null)
                if (!userSubscription.IsInactive)
                    return new() { Success = false, Message = $"User is already subscribe with {notificationWay.WayName}" };
                else
                {
                    userSubscription.IsInactive = false;
                    userSubscription.LastModifiedAt = DateTime.Now;
                    userSubscription.LastModifiedBy = user.Id.ToString();
                    await _notificationWayRepository.UpdateSubscriptionAsync(userSubscription);
                    return new() { Success = true, Message = "Subscription reactivated successfully" };
                }

            //chkeck if user has contact method related
            if (notificationWay.WayName == NotificationWayEnum.Email.ToString() && string.IsNullOrEmpty(user.Email))
                return new() { Success = false, Message = "You don't have email yet, please add an email first." };

            if (notificationWay.WayName == NotificationWayEnum.Email.ToString() && !string.IsNullOrEmpty(user.Email) && !user.EmailConfirmed)
                return new() { Success = false, Message = "Your email is not confirmed, please confirm email first;" };
            
            if (notificationWay.WayName == NotificationWayEnum.SMS.ToString() && string.IsNullOrEmpty(user.PhoneNumber))
                return new() { Success = false, Message = "You don't have phone number yet, please add an email first." };

            if (notificationWay.WayName == NotificationWayEnum.SMS.ToString() && !string.IsNullOrEmpty(user.PhoneNumber) && !user.PhoneNumberConfirmed)
                return new() { Success = false, Message = "Your phone number is not confirmed, please confirm phone number first;" };

            //subscribe
            userSubscription = new()
            {
                Id = new Guid(),
                User = user,
                UserId = user.Id,
                NotificationWay = notificationWay,
                NotificationWayId = notificationWay.Id,
                IsInactive = false,
                CreatedAt = DateTime.Now,
                CreatedBy = user.Id.ToString()
            };

            await _notificationWayRepository.AddSubscriptionAsync(userSubscription);

            return new() { Success = true, Message = "Subscription added successfully" };
        }

        public async Task<NotificationWayUnsubscriptionResponse> UnsubscribeNotificationWayCommandAsync(NotificationWaySubsciptionCommand request)
        {
            throw new NotImplementedException();
        }
    }
}
