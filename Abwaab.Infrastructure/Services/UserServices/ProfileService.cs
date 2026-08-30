using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Profile.Email;
using Abwaab.Application.Common.Exceptions.Profile.NotificationWay;
using Abwaab.Application.Common.Exceptions.Profile.Phone;
using Abwaab.Application.Common.Mappings;
using Abwaab.Application.Contracts;
using Abwaab.Application.Features.Users.Profile.NotificationWaySubscription.Subscribe;
using Abwaab.Application.Features.Users.Profile.NotificationWaySubscription.Unsubscribe;
using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.NotificationEntities;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Domain.Enums;
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

        public async Task<List<UserNotificationSubscription>> GetAllUserNotificationWaysAsync(Guid userId)
        {
            return await _notificationWayRepository.GetAllNotificationWaysOfUserAsync(userId);
        }

        public async Task<NotificationWay> FindNotificationWayByNameAsync(NotificationWaysEnum wayName, string errorTitle)
        {
            NotificationWay? notificationWay = await _notificationWayRepository.FindNotificationWayByNameAsync(wayName.ToString());
            if (notificationWay == null)
                throw new NotFoundException(
                    nameof(NotificationWay),
                    nameof(notificationWay.WayName),
                    wayName.ToString(),
                    errorTitle);

            return notificationWay;
        }

        public async Task<bool>HasUserActiveNotificationWay(Guid userId, Guid notifiacationWayId, string errorTitle)
        {
            return await _notificationWayRepository.HasUserActiveNotificationWay(userId, notifiacationWayId);
        }

        public async Task<bool> HasActivatedEmailNotificationWay(Guid userId, string errorTitle)
        {
            NotificationWay emailNotifictionWay = await FindNotificationWayByNameAsync(NotificationWaysEnum.Email, errorTitle);

            return await HasUserActiveNotificationWay(userId, emailNotifictionWay.Id, errorTitle);
        }

        public async Task<bool> HasActivatedSmsNotificationWay(Guid userId, string errorTitle)
        {
            NotificationWay emailNotifictionWay = await FindNotificationWayByNameAsync(NotificationWaysEnum.SMS, errorTitle);

            return await HasUserActiveNotificationWay(userId, emailNotifictionWay.Id, errorTitle);
        }

        public async Task<bool> HasActivatedWebNotificationWay(Guid userId, string errorTitle)
        {
            NotificationWay emailNotifictionWay = await FindNotificationWayByNameAsync(NotificationWaysEnum.Web_Application, errorTitle);

            return await HasUserActiveNotificationWay(userId, emailNotifictionWay.Id, errorTitle);
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
              
        public async Task SubscribeNotificationWayCommandAsync(ApplicationUser user, NotificationWaysEnum notificationWayType)
        {
            NotificationWay? notificationWay = await _notificationWayRepository.FindNotificationWayByNameAsync(notificationWayType.ToString());

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
                    await _userManager.UpdateAsync(user);
                }
            }
        }

        public async Task<NotificationWaySubscriptionResponse> SubscribeNotificationWayCommandAsync(ApplicationUser user, Guid notificationWayId, string errorTitle)
        {
            //check if notification way exist
            NotificationWay? notificationWay = await _notificationWayRepository.GetNotificationWayByIdAsync(notificationWayId);
            if (notificationWay == null)
                throw new NotFoundException(nameof(NotificationWay), nameof(notificationWayId), notificationWayId.ToString(), errorTitle);

            //check if user already has subscription
            UserNotificationSubscription? userSubscription = await _notificationWayRepository.GetUserSubscriptionAsync(user.Id, notificationWayId);

            if (userSubscription != null)
                if (!userSubscription.IsInactive)
                    throw new AlreadySubscribeNotificationWayException(notificationWay.WayName, errorTitle);
                else
                {
                    userSubscription.IsInactive = false;
                    userSubscription.LastModifiedAt = DateTime.Now;
                    userSubscription.LastModifiedBy = user.Id.ToString();
                    await _notificationWayRepository.UpdateSubscriptionAsync(userSubscription);
                    return new() { Success = true, Message = "تم تفعيل الاشتراك بطريقة الاشعار بنجاح" };
                }

            //chkeck if user has contact method related
            if (notificationWay.WayName == NotificationWaysEnum.Email.ToString() && string.IsNullOrEmpty(user.Email))
                throw new NoRegisterdEmailException(errorTitle);

            if (notificationWay.WayName == NotificationWaysEnum.Email.ToString() && !string.IsNullOrEmpty(user.Email) && !user.EmailConfirmed)
                throw new NoVerifiedEmailException(errorTitle);

            if (notificationWay.WayName == NotificationWaysEnum.SMS.ToString() && string.IsNullOrEmpty(user.PhoneNumber))
                throw new NoRegisterdPhoneException(errorTitle);

            if (notificationWay.WayName == NotificationWaysEnum.SMS.ToString() && !string.IsNullOrEmpty(user.PhoneNumber) && !user.PhoneNumberConfirmed)
                throw new NoVerifiedPhoneException(errorTitle);

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

            return new() { Success = true, Message = "تمت إضافة طريقة الإشعار بنجاح" };
        }

        public async Task<NotificationWayUnsubscriptionResponse> UnsubscribeNotificationWayCommandAsync(ApplicationUser user, Guid notificationWayId, string errorTitle)
        {
            //check if notification way exist
            NotificationWay? notificationWay = await _notificationWayRepository.GetNotificationWayByIdAsync(notificationWayId);
            if (notificationWay == null)
                throw new NotFoundException(nameof(NotificationWay), nameof(notificationWayId), notificationWayId.ToString(), errorTitle);

            if(!notificationWay.CanDisable)
                throw new UnsubscriptionNotificationWayNotAllowedException(NotificationWaysMapping.Map(notificationWay.WayName) ,errorTitle);

            //check if user had already subscribe
            UserNotificationSubscription? userSubscription = await _notificationWayRepository.GetUserSubscriptionAsync(user.Id, notificationWayId);

            if (userSubscription == null || userSubscription.IsInactive)
                throw new AlreadyUnsubscribeNotificationWayException(
                    NotificationWaysMapping.Map(notificationWay.WayName), 
                    errorTitle);

            userSubscription.IsInactive = true;
            userSubscription.LastModifiedAt = DateTime.Now;
            userSubscription.LastModifiedBy = user.Id.ToString();
            await _notificationWayRepository.UpdateSubscriptionAsync(userSubscription);
            return new() { Success = true, Message = "تم إلغاء تفعيل طريقة الاشعار بنجاح" };
        }

        
    }
}
