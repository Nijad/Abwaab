using Abwaab.Application.Common.Contracts;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Interfaces;
using Abwaab.Application.Features.Users.Profile.Email.Cancel;
using Abwaab.Application.Features.Users.Profile.Email.Confirm;
using Abwaab.Application.Features.Users.Profile.Email.InitiateChange;
using Abwaab.Application.Features.Users.Profile.Email.Pending;
using Abwaab.Application.Features.Users.Profile.NotificationWaySubscription.Subscribe;
using Abwaab.Application.Features.Users.Profile.NotificationWaySubscription.Unsubscribe;
using Abwaab.Application.Features.Users.Profile.Password.Change;
using Abwaab.Application.Features.Users.Profile.Password.Forgot;
using Abwaab.Application.Features.Users.Profile.Phone.Cancel;
using Abwaab.Application.Features.Users.Profile.Phone.Confirm;
using Abwaab.Application.Features.Users.Profile.Phone.InitiateChange;
using Abwaab.Application.Features.Users.Profile.Phone.Pending;
using Abwaab.Domain.Entities.NotificationEntities;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Domain.Enums;
using Abwaab.Infrastructure.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace Abwaab.Infrastructure.Services.UserServices
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMemoryCache _cache;
        private readonly IUserService _userService;
        private readonly INotificationWayRepository _notificationWayRepository;
        private readonly IUserContext _userContext;
        private readonly IRefreshTokenRepository _refreshTokenRepo;
        private readonly ILogger<ProfileService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenBlacklistService _blacklistService;
        private readonly IEmailSender _emailSender;
        private readonly IVerificationCodeService _verificationService;
        private readonly IUrlBuilder _urlBuilder;
        private readonly ISmsSender _smsSender;
        public ProfileService(
            UserManager<ApplicationUser> userManager,
            IUserService userService,
            INotificationWayRepository notificationWayRepository,
            IMemoryCache cache,
            IUserContext userContext,
            IRefreshTokenRepository refreshTokenRepo,
            ILogger<ProfileService> logger,
            IHttpContextAccessor httpContextAccessor,
            ITokenBlacklistService blacklistService,
            IEmailSender emailSender,
            IVerificationCodeService verificationService,
            IUrlBuilder urlBuilder,
            ISmsSender smsSender)
        {
            _userManager = userManager;
            _userService = userService;
            _notificationWayRepository = notificationWayRepository;
            _cache = cache;
            _userContext = userContext;
            _refreshTokenRepo = refreshTokenRepo;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _blacklistService = blacklistService;
            _emailSender = emailSender;
            _verificationService = verificationService;
            _urlBuilder = urlBuilder;
            _smsSender = smsSender;
        }

        public async Task<CancelEmailChangeResponse> CancelEmailChangeCommandAsync(CancelEmailChangeCommand request)
        {
            var userId = _userContext.UserId;

            // 1. Check if there is a pending change
            var cacheKey = $"email_change_{userId}";
            if (!_cache.TryGetValue(cacheKey, out PendingEmailChange pending))
            {
                return new CancelEmailChangeResponse { Success = false, Message = "No pending email change found." };
            }

            // 2. Remove the pending change from cache
            _cache.Remove(cacheKey);

            // 3. Revoke ALL refresh tokens (force logout on all devices)
            var activeTokens = await _refreshTokenRepo.GetActiveTokensForUserAsync(userId);
            foreach (var token in activeTokens)
            {
                token.IsRevoked = true;
                token.RevokedByIp = "Cancelled by user";
                await _refreshTokenRepo.UpdateAsync(token);
            }

            _logger.LogWarning("Email change cancelled by user {UserId}. All sessions revoked.", userId);

            return new CancelEmailChangeResponse { Success = true, Message = "Pending change cancelled. You have been logged out for security." };
        }

        public async Task<CancelPhoneChangeResponse> CancelPhoneChangeCommandAsync(CancelPhoneChangeCommand request)
        {
            var userId = _userContext.UserId;
            var cacheKey = $"phone_change_{userId}";

            if (!_cache.TryGetValue(cacheKey, out PendingPhoneChange pending))
            {
                return new CancelPhoneChangeResponse { Success = false, Message = "No pending phone change found." };
            }

            _cache.Remove(cacheKey);

            // Revoke all refresh tokens (force logout on all devices)
            var activeTokens = await _refreshTokenRepo.GetActiveTokensForUserAsync(userId);
            foreach (var token in activeTokens)
            {
                token.IsRevoked = true;
                token.RevokedByIp = "Cancelled by user";
                await _refreshTokenRepo.UpdateAsync(token);
            }

            _logger.LogWarning("Phone change cancelled by user {UserId}. All sessions revoked.", userId);
            return new CancelPhoneChangeResponse { Success = true, Message = "Pending change cancelled. You have been logged out for security." };
        }

        public async Task<ChangePasswordResponse> ChangePasswordCommandAsync(ChangePasswordDTO request)
        {
            var userId = _userContext.UserId;
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return new ChangePasswordResponse { Success = false, Message = "User not found." };

            // 1. Change the password
            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new ChangePasswordResponse { Success = false, Message = $"Failed: {errors}" };
            }

            // null = unlock immediately
            await _userManager.SetLockoutEndDateAsync(user, null);
            // Reset failed attempts to 0
            await _userManager.ResetAccessFailedCountAsync(user);

            var jti = _httpContextAccessor.HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Jti);
            if (!string.IsNullOrEmpty(jti))
            {
                var expClaim = _httpContextAccessor.HttpContext.User.FindFirst("exp")?.Value;
                if (long.TryParse(expClaim, out var exp))
                {
                    var expiry = DateTimeOffset.FromUnixTimeSeconds(exp).UtcDateTime;
                    _blacklistService.AddToBlacklist(jti, expiry);
                }
            }

            // Revoke all refresh tokens
            var tokens = await _refreshTokenRepo.GetActiveTokensForUserAsync(userId);
            foreach (var token in tokens)
            {
                token.IsRevoked = true;
                token.RevokedByIp = "Password changed";
                await _refreshTokenRepo.UpdateAsync(token);
            }

            // Delete the refresh token cookie
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("RefreshToken");

            _logger.LogInformation("Password changed successfully for user {UserId}. All sessions revoked.", userId);

            // Optionally, you can also add a security alert here (like we did for email/phone)
            // _ = SendSecurityAlertAsync(user.Email, "Your password was changed");

            _ = Task.Run(async () =>
            {
                //todo: check if user has email
                var subject = "Security Alert: Your Password Was Changed";
                var body = $@"
                    <h2>Password Changed</h2>
                    <p>Your account password was recently changed.</p>
                    <p><strong>Date/Time:</strong> {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC</p>
                    <p><strong>IP Address:</strong> {_userContext.RemoteIpAddress}</p>
                    <p>If you did NOT make this change, please reset your password immediately.</p>
                ";
                await _emailSender.SendEmailAsync(user.Email, subject, body);
                //todo: check if user has phone send alert sms
            });

            return new ChangePasswordResponse { Success = true, Message = "Password changed successfully. You have been logged out of all other devices." };
        }

        public async Task<ConfirmEmailChangeResponse> ConfirmEmailChangeCommandAsync(ConfirmEmailChangeCommand request)
        {
            var userId = _userContext.UserId;
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return new ConfirmEmailChangeResponse { Success = false, Message = "User not found." };

            // Retrieve the pending change from cache
            var cacheKey = $"email_change_{userId}";
            if (!_cache.TryGetValue(cacheKey, out PendingEmailChange pending))
                return new ConfirmEmailChangeResponse { Success = false, Message = "No pending email change request found. Please initiate again." };

            // Validate the code and the new email
            if (pending.Code != request.Code || pending.NewEmail != request.NewEmail)
                return new ConfirmEmailChangeResponse { Success = false, Message = "Invalid code or email mismatch." };

            // Check again if the email is still available (in case someone else took it while waiting)
            var existingUser = await _userManager.FindByEmailAsync(request.NewEmail);
            if (existingUser != null && existingUser.Id != userId)
                return new ConfirmEmailChangeResponse { Success = false, Message = "Email is already in use by another account." };

            // Store the old email before overwriting
            user.PreviousEmail = user.Email;
            // Update the user's email
            user.Email = request.NewEmail;
            // If you use email as username
            user.UserName = request.NewEmail;
            // Force re-verification of the new email
            //user.EmailConfirmed = false;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new ConfirmEmailChangeResponse { Success = false, Message = $"Update failed: {errors}" };
            }

            // Remove the cache entry (one-time use)
            _cache.Remove(cacheKey);

            _logger.LogInformation($"Email changed successfully for user {userId} to {request.NewEmail}", userId, request.NewEmail);

            return new ConfirmEmailChangeResponse { Success = true, Message = $"Email address changed successfully. Verification code sent to your new email {request.NewEmail}" };
        }

        public async Task<ConfirmPhoneNoChangeResponse> ConfirmPhoneNoChangeCommandAsync(ConfirmPhoneNoChangeCommand request)
        {
            var userId = _userContext.UserId;
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return new ConfirmPhoneNoChangeResponse { Success = false, Message = "User not found." };

            var cacheKey = $"phone_change_{userId}";
            if (!_cache.TryGetValue(cacheKey, out PendingPhoneChange pending))
                return new ConfirmPhoneNoChangeResponse { Success = false, Message = "No pending phone change request found. Please initiate again." };

            if (pending.Code != request.Code || pending.NewPhoneNo != request.NewPhoneNo)
                return new ConfirmPhoneNoChangeResponse { Success = false, Message = "Invalid code or phone number mismatch." };

            // Double-check if the phone number is still available (race condition)
            var existingUser = await _userManager.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == request.NewPhoneNo);
            if (existingUser != null && existingUser.Id != userId)
                return new ConfirmPhoneNoChangeResponse { Success = false, Message = "Phone number is already in use by another account." };

            // Store the old phone
            user.PreviousPhoneNumber = user.PhoneNumber;
            // Update the user's phone number
            user.PhoneNumber = request.NewPhoneNo;
            // Force re-verification
            //user.PhoneNumberConfirmed = false;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Phone update failed for user {UserId}: {Errors}", userId, errors);
                return new ConfirmPhoneNoChangeResponse { Success = false, Message = $"Update failed: {errors}" };
            }

            // Remove the cache entry (one-time use)
            _cache.Remove(cacheKey);

            _logger.LogInformation("Phone number changed successfully for user {UserId} to {NewPhone}", userId, request.NewPhoneNo);
            return new ConfirmPhoneNoChangeResponse { Success = true, Message = "Phone number updated successfully." };
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordCommandAsyn(ForgotPasswordDTO request)
        {
            var user = await _userService.FindUserByIdentifierAsync(request.Identifier, request.IdentifierType);
            if (user == null)
                throw new NotFoundException("User", request.IdentifierType.ToString().Replace('_', ' '), request.Identifier);
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, resetToken, request.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));

                return await Task.FromResult(new ForgotPasswordResponse { Success = false, Message = $"Password reset failed: {errors}" });
            }

            // null = unlock immediately
            await _userManager.SetLockoutEndDateAsync(user, null);
            // Reset failed attempts to 0
            await _userManager.ResetAccessFailedCountAsync(user);

            var activeTokens = await _refreshTokenRepo.GetActiveTokensForUserAsync(user.Id);
            foreach (var token in activeTokens)
            {
                token.IsRevoked = true;
                token.RevokedByIp = "Password reset";
                await _refreshTokenRepo.UpdateAsync(token);
            }

            return new ForgotPasswordResponse { Success = true, Message = "Password reset successful." };
        }

        public async Task<InitiatePhoneNoChangeResponse> InitiatePhoneNoChangeCommandAsync(InitiatePhoneNoChangeCommand request)
        {
            var userId = _userContext.UserId;
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return new InitiatePhoneNoChangeResponse { Success = false, Message = "User not found." };

            // ----------------------------------------------------------------
            // 1. SECURITY: Verify the user's current password (Critical!)
            // ----------------------------------------------------------------
            if (!await _userManager.CheckPasswordAsync(user, request.CurrentPassword))
                return new InitiatePhoneNoChangeResponse { Success = false, Message = "Incorrect current password." };

            // Check if new phone is the same as current
            if (user.PhoneNumber?.Equals(request.NewPhoneNo, StringComparison.OrdinalIgnoreCase) == true)
                return new InitiatePhoneNoChangeResponse { Success = false, Message = "This is already your current phone number." };

            // Check if the new phone number is already taken by another user
            var existingUser = await _userManager.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == request.NewPhoneNo);
            if (existingUser != null && existingUser.Id != userId)
                return new InitiatePhoneNoChangeResponse { Success = false, Message = "Phone number is already in use by another account." };

            // ----------------------------------------------------------------
            // 2. NOTIFY THE OLD CONTACT (Security Alert)
            //    Send an SMS to the OLD number to alert the user.
            //    Fire-and-forget (background) so we don't slow down the response.
            // ----------------------------------------------------------------

            var cancelUrl = _urlBuilder.GetCancelPhoneChangeUrl();
            if (!string.IsNullOrEmpty(user.PhoneNumber))
                _ = Task.Run(async () =>
                {
                    var alertMessage = $"SECURITY ALERT: Your phone is being changed to {request.NewPhoneNo}. If this wasn't you, cancel at {cancelUrl}";

                    await _smsSender.SendSmsAsync(user.PhoneNumber, alertMessage);
                });

            if (!string.IsNullOrEmpty(user.Email))
                _ = Task.Run(async () =>
                {
                    var alertSubject = "Security Alert: Phone Change Requested";
                    var alertBody = $@"
                        <h2>Security Alert: Phone Change Requested</h2>
                        <p>We received a request to change the phone number associated with your account.</p>
                        <p><strong>New phone requested:</strong> {request.NewPhoneNo}</p>
                        <p><strong>Time:</strong> {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC</p>
                        <p>If you made this request, please enter the verification code sent to your new phone number.</p>
                        <p><strong>If you did NOT request this, click the link below to cancel the change immediately:</strong></p>
                        <p><a href='{cancelUrl}'>Cancel Phone Change</a></p>
                        <p>This link will revoke all your active sessions for security.</p>
                    ";
                    await _emailSender.SendEmailAsync(user.Email, alertSubject, alertBody);
                });

            // ----------------------------------------------------------------
            // 3. VERIFY THE NEW CONTACT
            //    Send a verification code to the NEW phone number.
            // ----------------------------------------------------------------
            var code = _verificationService.GenerateVerificationCode();
            var sent = await _verificationService.SendVerificationCodeViaSmsAsync(request.NewPhoneNo, code);
            if (!sent)
                return new InitiatePhoneNoChangeResponse { Success = false, Message = "Failed to send verification code to new phone number. Please try again." };

            // Store the pending change in cache (valid for 5 minutes)
            var pending = new PendingPhoneChange
            {
                NewPhoneNo = request.NewPhoneNo,
                Code = code,
                CreatedAt = DateTime.UtcNow
            };
            _cache.Set($"phone_change_{userId}", pending, TimeSpan.FromMinutes(5));

            _logger.LogInformation("Phone change initiated for user {UserId} to {NewPhone}. Alert sent to old number.", userId, request.NewPhoneNo);

            return new InitiatePhoneNoChangeResponse { Success = true, Message = "Verification code sent to the new phone number." };
        }

        public async Task<InitiateEmailChangeResponse> InitiatieEmailChangeCommandAsync(InitiateEmailChangeCommand request)
        {
            var userId = _userContext.UserId;
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return new InitiateEmailChangeResponse { Success = false, Message = "User not found." };

            // ----------------------------------------------------------------
            // 1. SECURITY: Verify the user's current password (Critical!)
            // ----------------------------------------------------------------
            if (!await _userManager.CheckPasswordAsync(user, request.CurrentPassword))
                return new InitiateEmailChangeResponse { Success = false, Message = "Incorrect current password." };

            // Check if new email is the same as current
            if (user.Email?.Equals(request.NewEmail, StringComparison.OrdinalIgnoreCase) == true)
                return new InitiateEmailChangeResponse { Success = false, Message = "This is already your current email." };

            // Check if new email is taken by another account
            var existingUser = await _userManager.FindByEmailAsync(request.NewEmail);
            if (existingUser != null && existingUser.Id != userId)
                return new InitiateEmailChangeResponse { Success = false, Message = "Email is already in use." };

            // ----------------------------------------------------------------
            // 2. NOTIFY THE OLD CONTACT (Security Alert)
            //    Send an email to the OLD address to alert the user.
            //    This runs in the background (fire and forget) so we don't slow down the response.
            // ----------------------------------------------------------------
            var cancelUrl = _urlBuilder.GetCancelEmailChangeUrl();
            if (!string.IsNullOrEmpty(user.PhoneNumber))
                _ = Task.Run(async () =>
                {
                    var alertMessage = $"SECURITY ALERT: Your email is being changed to {request.NewEmail}. If this wasn't you, cancel at {cancelUrl}";

                    await _smsSender.SendSmsAsync(user.PhoneNumber, alertMessage);
                });

            if (!string.IsNullOrEmpty(user.Email))
                _ = Task.Run(async () =>
                {
                    var alertSubject = "Security Alert: Email Change Requested";
                    var alertBody = $@"
                        <h2>Security Alert: Email Change Requested</h2>
                        <p>We received a request to change the email address associated with your account.</p>
                        <p><strong>New email requested:</strong> {request.NewEmail}</p>
                        <p><strong>Time:</strong> {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC</p>
                        <p>If you made this request, please enter the verification code sent to your new email.</p>
                        <p><strong>If you did NOT request this, click the link below to cancel the change immediately:</strong></p>
                        <p><a href='{cancelUrl}'>Cancel Email Change</a></p>
                        <p>This link will revoke all your active sessions for security.</p>
                    ";
                    await _emailSender.SendEmailAsync(user.Email, alertSubject, alertBody);
                });

            // ----------------------------------------------------------------
            // 3. VERIFY THE NEW CONTACT
            //    Send a verification code to the NEW email.
            // ----------------------------------------------------------------
            var code = _verificationService.GenerateVerificationCode();
            var sent = await _verificationService.SendVerificationCodeViaEmailAsync(request.NewEmail, code);
            if (!sent)
                return new InitiateEmailChangeResponse { Success = false, Message = "Failed to send verification code to new email." };

            // Store pending change
            var pending = new PendingEmailChange
            {
                NewEmail = request.NewEmail,
                Code = code,
                CreatedAt = DateTime.UtcNow
            };
            _cache.Set($"email_change_{userId}", pending, TimeSpan.FromMinutes(Constants.CODE_TIMEOUT_MINUTES));

            _logger.LogInformation($"Email change initiated for user {userId} to {request.NewEmail}. Alert sent to old email.", userId, request.NewEmail);

            // We don't mention the alert to the user to avoid confusion, but it's sent.
            return new InitiateEmailChangeResponse { Success = true, Message = "Verification code sent to the new email address." };
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
