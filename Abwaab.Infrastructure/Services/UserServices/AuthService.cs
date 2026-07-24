using Abwaab.Application.Common.Contracts;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Interfaces;
using Abwaab.Application.DTOs.ApplicationUser;
using Abwaab.Application.DTOs.ApplicationUser.ChangePassword;
using Abwaab.Application.DTOs.ApplicationUser.ForgotPassword;
using Abwaab.Application.DTOs.ApplicationUser.LoginUser;
using Abwaab.Application.DTOs.ApplicationUser.LogoutUser;
using Abwaab.Application.DTOs.ApplicationUser.RegisterUser;
using Abwaab.Application.DTOs.ApplicationUser.VerificationCode;
using Abwaab.Domain.Entities.NotificationEntities;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Domain.Enums;
using Abwaab.Infrastructure.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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

        public AuthService(
            IVerificationCodeService verificationService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtService jwtService,
            IRefreshTokenRepository refreshTokenRepo,
            IOptions<JwtSettings> jwtSettings,
            ILogger<AuthService> logger,
            IUserContext userContext,
            INotificationWayRepository notificationWayRepository)
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
        }

        private async Task<ApplicationUser?> FinedUserByIdentifierAsync(string identifier, IdentifierEnum identifierType)
        {
            ApplicationUser? user = null;
            if (identifierType == IdentifierEnum.email)
            {
                user = await _userManager.FindByEmailAsync(identifier);
            }
            else if (identifierType == IdentifierEnum.phone_number)
            {
                user = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.PhoneNumber == identifier);
            }
            return user;
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
            var accessToken = _jwtService.GenerateAccessToken(user);
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

            var code = _verificationService.GenerateCode();

            Task<bool> sent = Task.FromResult(false);

            if (registerDTO.IdentifierType == IdentifierEnum.email)
                sent = _verificationService.SendVerificationCodeViaEmailAsync(registerDTO.Identifier, code);
            else if (registerDTO.IdentifierType == IdentifierEnum.phone_number)
                sent = _verificationService.SendVerificationCodeViaSmsAsync(registerDTO.Identifier, code);

            if (!sent.Result)
                return await Task.FromResult(new RegisterUserResponse(false, "Failed to send verification code."));

            return await Task.FromResult(new RegisterUserResponse(true, $"Register Successful, Verification code sent to your {registerDTO.IdentifierType.ToString().Replace('_', ' ')}"));
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

            return new ForgotPasswordResponse { Success = true, Message = "Password reset successful." };
        }

        public async Task<ChangePasswordResponse> ChangePassword(ChangePasswordDTO request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
                return new ChangePasswordResponse { Success = false, Message = "User not found." };

            var result = await _userManager.ChangePasswordAsync(
                user, request.CurrentPassword,
                request.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new ChangePasswordResponse { Success = false, Message = $"Failed: {errors}" };
            }

            _logger.LogInformation("Password changed for user {UserId}", request.UserId.ToString());

            return new ChangePasswordResponse { Success = true, Message = "Password changed successfully." };
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
    }
}
