using Abwaab.Application.Common.Contracts;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Interfaces;
using Abwaab.Application.DTOs.ApplicationUser;
using Abwaab.Application.DTOs.ApplicationUser.ChangePassword;
using Abwaab.Application.DTOs.ApplicationUser.ForgotPassword;
using Abwaab.Application.DTOs.ApplicationUser.IdentifierManagement;
using Abwaab.Application.DTOs.ApplicationUser.LoginUser;
using Abwaab.Application.DTOs.ApplicationUser.LogoutUser;
using Abwaab.Application.DTOs.ApplicationUser.RegisterUser;
using Abwaab.Application.DTOs.ApplicationUser.VerificationCode;
using Abwaab.Domain.Entities.NotificationEntities;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Domain.Enums;
using Abwaab.Infrastructure.Common;
using Abwaab.Infrastructure.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using IEmailSender = Abwaab.Application.Common.Interfaces.IEmailSender;

namespace Abwaab.Infrastructure.Services.UserServices
{
    public class AuthService : IAuthService
    {
        private readonly IVerificationCodeService _verificationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOptions<JwtSettings> _jwtSettings;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenRepository _refreshTokenRepo;
        private readonly ILogger<AuthService> _logger;
        private readonly IUserContext _userContext;
        private readonly INotificationWayRepository _notificationWayRepository;
        private readonly IMemoryCache _cache;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly IUrlBuilder _urlBuilder;

        public AuthService(
            IVerificationCodeService verificationService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtService jwtService,
            IRefreshTokenRepository refreshTokenRepo,
            IOptions<JwtSettings> jwtSettings,
            ILogger<AuthService> logger,
            IUserContext userContext,
            INotificationWayRepository notificationWayRepository,
            IMemoryCache cache,
            IEmailSender emailSender,
            ISmsSender smsSender,
            IUrlBuilder urlBuilder)
        {
            _verificationService = verificationService;
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _refreshTokenRepo = refreshTokenRepo;
            _logger = logger;
            _userContext = userContext;
            _notificationWayRepository = notificationWayRepository;
            _cache = cache;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _urlBuilder = urlBuilder;
        }

        private async Task<ApplicationUser?> FinedUserByIdentifierAsync(string identifier, IdentifierEnum identifierType, bool lookAtPrevious = false)
        {
            ApplicationUser? user = null;
            if (identifierType == IdentifierEnum.email)
            {
                user = await _userManager.FindByEmailAsync(identifier);
                if (user != null)
                    return user;
                return await _userManager.Users
                    .FirstOrDefaultAsync(u => u.PreviousEmail == identifier);
            }
            else if (identifierType == IdentifierEnum.phone_number)
            {
                user = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.PhoneNumber == identifier);
                if (user != null)
                    return user;
                return await _userManager.Users
                    .FirstOrDefaultAsync(u => u.PreviousPhoneNumber == identifier);
            }

            throw new NotImplementedException($"Identifier type of {identifierType.ToString()} does not implemented yet");
        }

        public async Task<LoginUserResponse> LoginUserAsync(LoginUserDTO loginUserDTO)
        {
            bool confirmed = false;

            // Find user by email or phone
            ApplicationUser? user = FinedUserByIdentifierAsync(loginUserDTO.Identifier, loginUserDTO.IdentifierType).Result;

            if (user == null)
                throw new NotFoundException("User", loginUserDTO.IdentifierType.ToString().Replace('_', ' '), loginUserDTO.Identifier);

            // Check password
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginUserDTO.Password, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                throw new InvalidPasswordException();
            }

            if (loginUserDTO.IdentifierType == IdentifierEnum.email)
                confirmed = user.EmailConfirmed;
            else if (loginUserDTO.IdentifierType == IdentifierEnum.phone_number)
                confirmed = user.PhoneNumberConfirmed;

            if (!confirmed)
                return await Task.FromResult(new LoginUserResponse { Success = false, Message = $"Please verify your {loginUserDTO.IdentifierType.ToString().Replace('_', ' ')} before logging in." });

            // Generate access token
            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _jwtService.GenerateAccessToken(user, roles);
            var refreshTokenString = _jwtService.GenerateRefreshToken();

            // Store refresh token
            var refreshToken = new RefreshToken
            {
                Token = refreshTokenString,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(_jwtSettings.Value.RefreshTokenExpiryDays),
                IsRevoked = false,
                CreatedAt = DateTime.UtcNow
            };
            await _refreshTokenRepo.CreateAsync(refreshToken);

            _logger.LogInformation("User {UserId} logged in successfully.", user.Id);

            return new LoginUserResponse
            {
                Success = true,
                AccessToken = accessToken,
                RefreshToken = refreshTokenString,
                ExpiresIn = _jwtSettings.Value.AccessTokenExpiryMinutes * 60,
                Message = "Login successful",
            };
        }

        public async Task<RegisterUserResponse> RegisterUserAsync(RegisterUserDTO registerDTO)
        {
            ApplicationUser? getUser = null;
            if (registerDTO.IdentifierType == IdentifierEnum.email)
            {
                getUser = await _userManager.FindByEmailAsync(registerDTO.Identifier);
            }

            if (registerDTO.IdentifierType == IdentifierEnum.phone_number)
            {
                getUser = await _userManager.Users
                                .FirstOrDefaultAsync(u => u.PhoneNumber == registerDTO.Identifier);
            }

            if (getUser != null)
                return await Task.FromResult(new RegisterUserResponse(false, "User already exists"));

            ApplicationUser newUser = new ApplicationUser
            {
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                UserName = registerDTO.Identifier,
            };

            if (registerDTO.IdentifierType == IdentifierEnum.email)
                newUser.Email = registerDTO.Identifier;
            else if (registerDTO.IdentifierType == IdentifierEnum.phone_number)
                newUser.PhoneNumber = registerDTO.Identifier;

            IdentityResult result = await _userManager.CreateAsync(newUser, registerDTO.Password);

            if (!result.Succeeded)
                return await Task.FromResult(new RegisterUserResponse(false, "Registration failed"));

            var code = _verificationService.GenerateVerificationCode();

            Task<bool> sent = Task.FromResult(false);

            if (registerDTO.IdentifierType == IdentifierEnum.email)
                sent = _verificationService.SendVerificationCodeViaEmailAsync(registerDTO.Identifier, code);
            else if (registerDTO.IdentifierType == IdentifierEnum.phone_number)
                sent = _verificationService.SendVerificationCodeViaSmsAsync(registerDTO.Identifier, code);

            if (!sent.Result)
                return new RegisterUserResponse(false, "Failed to send verification code.");

            return new RegisterUserResponse(true, $"Register Successful, Verification code sent to your {registerDTO.IdentifierType.ToString().Replace('_', ' ')}");
        }

        public async Task<VerifyCodeResponse> VerifyUserAsync(VerifyCodeDTO verifyCodeDTO)
        {
            bool isValid = _verificationService.VerifyCodeAsync(verifyCodeDTO.Identifier, verifyCodeDTO.Code).Result;

            if (!isValid)
                return await Task.FromResult(new VerifyCodeResponse { IsVerified = false, Message = "Invalid or expired verification code." });

            ApplicationUser? user = FinedUserByIdentifierAsync(verifyCodeDTO.Identifier, verifyCodeDTO.IdentifierType).Result;

            if (verifyCodeDTO.IdentifierType == IdentifierEnum.email)
            {
                var token = _userManager.GenerateEmailConfirmationTokenAsync(user).Result;

                var result = _userManager.ConfirmEmailAsync(user, token).Result;

                if (!result.Succeeded)
                    return await Task.FromResult(new VerifyCodeResponse { IsVerified = false, Message = "Failed to confirm email." });

                await MappingUserWithNotificationWay(user, NotificationWayEnum.Email);
            }
            else if (verifyCodeDTO.IdentifierType == IdentifierEnum.phone_number)
            {
                var token = _userManager.GenerateChangePhoneNumberTokenAsync(user, verifyCodeDTO.Identifier).Result;

                var result = _userManager.ChangePhoneNumberAsync(user, verifyCodeDTO.Identifier, token).Result;

                if (!result.Succeeded)
                    return await Task.FromResult(new VerifyCodeResponse { IsVerified = false, Message = "Failed to confirm phone number." });

                await MappingUserWithNotificationWay(user, NotificationWayEnum.SMS);
            }

            await MappingUserWithNotificationWay(user, NotificationWayEnum.Push_Notification);

            return await Task.FromResult(new VerifyCodeResponse { IsVerified = true, Message = "Verification successful." });
        }

        public async Task<bool> IsUserExistsAsync(IdentifierDTO resendCodeDTO)
        {
            if (FinedUserByIdentifierAsync(resendCodeDTO.Identifier, resendCodeDTO.IdentifierType).Result != null)
                return await Task.FromResult(true);

            if (resendCodeDTO.IdentifierType == IdentifierEnum.email && _userManager.FindByEmailAsync(resendCodeDTO.Identifier).Result != null)
                return await Task.FromResult(true);
            if (resendCodeDTO.IdentifierType == IdentifierEnum.phone_number && _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == resendCodeDTO.Identifier).Result != null)
                return await Task.FromResult(true);

            return await Task.FromResult(false);
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordAsyn(ForgotPasswordDTO request)
        {
            var user = FinedUserByIdentifierAsync(request.Identifier, request.IdentifierType).Result;
            if (user == null)
                throw new NotFoundException("User", request.IdentifierType.ToString().Replace('_', ' '), request.Identifier);
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, resetToken, request.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));

                return await Task.FromResult(new ForgotPasswordResponse { Success = false, Message = $"Password reset failed: {errors}" });
            }

            var activeTokens = await _refreshTokenRepo.GetActiveTokensForUserAsync(user.Id);
            foreach (var token in activeTokens)
            {
                token.IsRevoked = true;
                token.RevokedByIp = "Password reset";
                await _refreshTokenRepo.UpdateAsync(token);
            }

            return new ForgotPasswordResponse { Success = true, Message = "Password reset successful." };
        }

        public async Task<ChangePasswordResponse> ChangePassword(ChangePasswordDTO request)
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

            // 2. CRITICAL: Revoke ALL refresh tokens for this user (log out from all devices)
            var activeTokens = await _refreshTokenRepo.GetActiveTokensForUserAsync(userId);
            foreach (var token in activeTokens)
            {
                token.IsRevoked = true;
                token.RevokedByIp = "Password changed";
                await _refreshTokenRepo.UpdateAsync(token);
            }

            _logger.LogInformation("Password changed successfully for user {UserId}. All sessions revoked.", userId);

            // Optionally, you can also add a security alert here (like we did for email/phone)
            // _ = SendSecurityAlertAsync(user.Email, "Your password was changed");

            _ = Task.Run(async () =>
            {
                var subject = "Security Alert: Your Password Was Changed";
                var body = $@"
                    <h2>Password Changed</h2>
                    <p>Your account password was recently changed.</p>
                    <p><strong>Date/Time:</strong> {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC</p>
                    <p><strong>IP Address:</strong> {_userContext.RemoteIpAddress}</p>
                    <p>If you did NOT make this change, please reset your password immediately.</p>
                ";
                await _emailSender.SendEmailAsync(user.Email, subject, body);
            });

            return new ChangePasswordResponse { Success = true, Message = "Password changed successfully. You have been logged out of all other devices." };
        }

        public async Task<LogoutResponse> Logout(LogoutRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return new LogoutResponse { Success = false, Message = "User not found." };
            }

            if (request.RevokeAll)
            {
                // Revoke all active refresh tokens for this user
                var tokens = await _refreshTokenRepo.GetActiveTokensForUserAsync(request.UserId);
                foreach (var token in tokens)
                {
                    token.IsRevoked = true;
                    token.RevokedByIp = _userContext.RemoteIpAddress;
                    await _refreshTokenRepo.UpdateAsync(token);
                }
                _logger.LogInformation("All refresh tokens revoked for user {UserId}", request.UserId.ToString());
                return new LogoutResponse { Success = true, Message = "Logged out from all devices." };
            }
            else
            {
                var storedToken = await _refreshTokenRepo.GetByTokenAsync(request.RefreshToken);

                if (storedToken == null || storedToken.UserId != request.UserId)
                    return new LogoutResponse { Success = false, Message = "Invalid refresh token." };

                if (!storedToken.IsRevoked && storedToken.ExpiryDate > DateTime.UtcNow)
                {
                    storedToken.IsRevoked = true;
                    storedToken.RevokedByIp = _userContext.RemoteIpAddress;

                    await _refreshTokenRepo.UpdateAsync(storedToken);

                    _logger.LogInformation("Refresh token revoked for user {UserId}", request.UserId.ToString());

                    return new LogoutResponse { Success = true, Message = "Logged out successfully." };
                }
                else
                {
                    // Token is already expired or revoked
                    return new LogoutResponse { Success = false, Message = "Token already invalid." };
                }
            }
        }

        public async Task<bool> MappingUserWithNotificationWay(ApplicationUser user, NotificationWayEnum notificationWayType)
        {
            NotificationWay? notificationWay = _notificationWayRepository.GetNotificationWay(notificationWayType.ToString().Replace('_', ' ')).Result;

            if (notificationWay != null)
            {
                user.NotificationWaySubscriptions = await _notificationWayRepository.GetUserNotificationWays(user.Id);

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

        public async Task<InitiateEmailChangeResponse> InitiatieEmailChange(InitiateEmailChangeRequest request)
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

        public async Task<ConfirmEmailChangeResponse> ConfirmEmailChange(ConfirmEmailChangeRequest request)
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

        public async Task<InitiatePhoneNoChangeResponse> InitiatePhoneNoChange(InitiatePhoneNoChangeRequest request)
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


        public async Task<ConfirmPhoneNoChangeResponse> ConfirmPhoneNoChange(ConfirmPhoneNoChangeRequest request)
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

        public async Task<CancelEmailChangeResponse> CancelEmailChange(CancelEmailChangeRequest request)
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

        public async Task<CancelPhoneChangeResponse> CancelPhoneChange(CancelPhoneChangeRequest request)
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
    }
}
