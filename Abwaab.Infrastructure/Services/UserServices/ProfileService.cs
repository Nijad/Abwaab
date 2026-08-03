using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Profile.Email;
using Abwaab.Application.Common.Exceptions.Profile.NotificationWay;
using Abwaab.Application.Common.Exceptions.Profile.Phone;
using Abwaab.Application.Contracts;
using Abwaab.Application.Features.Users.Profile.NotificationWaySubscription.Subscribe;
using Abwaab.Application.Features.Users.Profile.NotificationWaySubscription.Unsubscribe;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.NotificationEntities;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Abwaab.Infrastructure.Services.UserServices
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotificationWayRepository _notificationWayRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepo;
        public ProfileService(
            UserManager<ApplicationUser> userManager,
            INotificationWayRepository notificationWayRepository,
            IRefreshTokenRepository refreshTokenRepo)
        {
            _userManager = userManager;
            _notificationWayRepository = notificationWayRepository;
            _refreshTokenRepo = refreshTokenRepo;
        }

        public async Task RevokeAllRefreshToken(Guid userId, string reason)
        {
            // Revoke ALL refresh tokens (force logout on all devices)
            var activeTokens = await _refreshTokenRepo.GetActiveTokensForUserAsync(userId);
            foreach (var token in activeTokens)
            {
                token.IsRevoked = true;
                token.RevokedByIp = reason;
                await _refreshTokenRepo.UpdateAsync(token);
            }
        }
              
        public async Task<bool> SubscribeNotificationWayCommandAsync(ApplicationUser user, NotificationWayEnum notificationWayType)
        {
            NotificationWay? notificationWay = await _notificationWayRepository.GetNotificationWayByNameAsync(notificationWayType.ToString().Replace('_', ' '));

            if (notificationWay != null)
            {
                user.NotificationWaySubscriptions = await _notificationWayRepository.GetNotificationWaysByUserAsync(user.Id);

                UserNotificationSubscription userNotificationWay = new()
                {
                    Id = new Guid(),
                    User = user,
                    UserId = user.Id,
                    NotificationWay = notificationWay,
                    NotificationWayId = notificationWay.Id,
                    IsInactive = false
                };

                if (!user.NotificationWaySubscriptions.Contains(userNotificationWay))
                {
                    user.NotificationWaySubscriptions.Add(userNotificationWay);
                    return _userManager.UpdateAsync(user).Result.Succeeded;
                }
            }

            return await Task.FromResult(false);
        }

        public async Task<NotificationWaySubscriptionResponse> SubscribeNotificationWayCommandAsync(ApplicationUser user, Guid notificationWayId)
        {
            //check if notification way exist
            NotificationWay? notificationWay = await _notificationWayRepository.GetNotificationWayByIdAsync(notificationWayId);
            if (notificationWay == null)
                throw new NotFoundException(nameof(NotificationWay), nameof(notificationWayId), notificationWayId.ToString());

            //check if user already has subscription
            UserNotificationSubscription? userSubscription = await _notificationWayRepository.GetUserSubscriptionAsync(user.Id, notificationWayId);

            if (userSubscription != null)
                if (!userSubscription.IsInactive)
                    throw new AlreadySubscribeNotificationWayException(notificationWay.WayName);
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
                throw new NoRegisterdEmailException();

            if (notificationWay.WayName == NotificationWayEnum.Email.ToString() && !string.IsNullOrEmpty(user.Email) && !user.EmailConfirmed)
                throw new NoVerifiedEmailException();

            if (notificationWay.WayName == NotificationWayEnum.SMS.ToString() && string.IsNullOrEmpty(user.PhoneNumber))
                throw new NoRegisterdPhoneException();

            if (notificationWay.WayName == NotificationWayEnum.SMS.ToString() && !string.IsNullOrEmpty(user.PhoneNumber) && !user.PhoneNumberConfirmed)
                throw new NoVerifiedPhoneException();

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

        public async Task<NotificationWayUnsubscriptionResponse> UnsubscribeNotificationWayCommandAsync(ApplicationUser user, Guid notificationWayId)
        {
            //check if notification way exist
            NotificationWay? notificationWay = await _notificationWayRepository.GetNotificationWayByIdAsync(notificationWayId);
            if (notificationWay == null)
                throw new NotFoundException(nameof(NotificationWay), nameof(notificationWayId), notificationWayId.ToString());

            //check if user had already subscribe
            UserNotificationSubscription? userSubscription = await _notificationWayRepository.GetUserSubscriptionAsync(user.Id, notificationWayId);

            if (userSubscription == null || !userSubscription.IsInactive)
                throw new AlreadyUnsubscribeNotificationWayException(notificationWay.WayName);

            userSubscription.IsInactive = true;
            userSubscription.LastModifiedAt = DateTime.Now;
            userSubscription.LastModifiedBy = user.Id.ToString();
            await _notificationWayRepository.UpdateSubscriptionAsync(userSubscription);
            return new() { Success = true, Message = "Subscription deactivated successfully" };
        }
    }
}
