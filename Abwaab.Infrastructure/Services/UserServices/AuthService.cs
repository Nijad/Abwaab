using Abwaab.Application.Common.Contracts;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Interfaces;
using Abwaab.Application.Features.Users.Auth.Login;
using Abwaab.Application.Features.Users.Auth.Logout;
using Abwaab.Application.Features.Users.Auth.Register;
using Abwaab.Application.Features.Users.Auth.SendCode;
using Abwaab.Application.Features.Users.Auth.VerificationCode;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Domain.Enums;
using Abwaab.Infrastructure.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using IEmailSender = Abwaab.Application.Common.Interfaces.IEmailSender;

namespace Abwaab.Infrastructure.Services.UserServices
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenBlacklistService _blacklistService;
        private readonly IProfileService _profileService;

        public AuthService(
            IUserService userService,
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
            IUrlBuilder urlBuilder,
            IHttpContextAccessor httpContextAccessor,
            ITokenBlacklistService blacklistService,
            IProfileService profileService)
        {
            _userService = userService;
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
            _httpContextAccessor = httpContextAccessor;
            _blacklistService = blacklistService;
            _profileService = profileService;
        }

        public async Task<LoginUserResponse> LoginUserCommandAsync(LoginUserDTO loginUserDTO)
        {
            bool confirmed = false;

            // Find user by email or phone
            ApplicationUser? user = await _userService.FindUserByIdentifierAsync(loginUserDTO.Identifier, loginUserDTO.IdentifierType);

            if (user == null)
                throw new NotFoundException("User", loginUserDTO.IdentifierType.ToString().Replace('_', ' '), loginUserDTO.Identifier);

            // Check password
            //var result = await _signInManager.CheckPasswordSignInAsync(user, loginUserDTO.Password, lockoutOnFailure: false);
            var result = await _signInManager.PasswordSignInAsync(user, loginUserDTO.Password, false, lockoutOnFailure: true);

            if (result.IsLockedOut)
                return new LoginUserResponse { Success = false, Message = "Account locked out." };

            if (!result.Succeeded)
                throw new InvalidPasswordException();

            if (loginUserDTO.IdentifierType == IdentifierEnum.email)
                confirmed = user.EmailConfirmed;
            else if (loginUserDTO.IdentifierType == IdentifierEnum.phone_number)
                confirmed = user.PhoneNumberConfirmed;

            if (!confirmed)
                return await Task.FromResult(new LoginUserResponse { Success = false, Message = $"Please verify your {loginUserDTO.IdentifierType.ToString().Replace('_', ' ')} before logging in." });

            // null = unlock immediately
            await _userManager.SetLockoutEndDateAsync(user, null);
            // Reset failed attempts to 0
            await _userManager.ResetAccessFailedCountAsync(user);

            // Generate access token
            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _jwtService.GenerateAccessToken(user, roles);
            var refreshTokenString = _jwtService.GenerateRefreshToken();
            var tokenHash = HashToken(refreshTokenString);
            // Store refresh token
            var refreshToken = new RefreshToken
            {
                TokenHash = tokenHash,
                //Token = refreshTokenString,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(_jwtSettings.Value.RefreshTokenExpiryDays),
                IsRevoked = false,
                CreatedAt = DateTime.UtcNow
            };
            await _refreshTokenRepo.CreateAsync(refreshToken);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,           // Not accessible via JavaScript
                Secure = true,             // Only sent over HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = refreshToken.ExpiryDate
            };

            _httpContextAccessor?.HttpContext?.Response.Cookies.Append("RefreshToken", refreshTokenString, cookieOptions);

            _logger.LogInformation("User {UserId} logged in successfully.", user.Id);

            return new LoginUserResponse
            {
                Success = true,
                AccessToken = accessToken,
                RefreshToken = tokenHash,
                ExpiresIn = _jwtSettings.Value.AccessTokenExpiryMinutes * 60,
                Message = "Login successful",
            };
        }

        private string HashToken(string token)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(bytes);
        }

        public async Task<RegisterUserResponse> RegisterUserCommandAsync(RegisterUserDTO registerDTO)
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
                LockoutEnabled = true
            };

            if (registerDTO.IdentifierType == IdentifierEnum.email)
                newUser.Email = registerDTO.Identifier;
            else if (registerDTO.IdentifierType == IdentifierEnum.phone_number)
                newUser.PhoneNumber = registerDTO.Identifier;

            IdentityResult result = await _userManager.CreateAsync(newUser, registerDTO.Password);

            if (!result.Succeeded)
                return await Task.FromResult(new RegisterUserResponse(false, "Registration failed"));

            var code = _verificationService.GenerateVerificationCode();

            bool sent = await Task.FromResult(false);

            if (registerDTO.IdentifierType == IdentifierEnum.email)
                sent = await _verificationService.SendVerificationCodeViaEmailAsync(registerDTO.Identifier, code);
            else if (registerDTO.IdentifierType == IdentifierEnum.phone_number)
                sent = await _verificationService.SendVerificationCodeViaSmsAsync(registerDTO.Identifier, code);

            if (!sent)
                return new RegisterUserResponse(false, "Failed to send verification code.");

            return new RegisterUserResponse(true, $"Register Successful, Verification code sent to your {registerDTO.IdentifierType.ToString().Replace('_', ' ')}");
        }

        public async Task<VerifyCodeResponse> VerifyUserCommandAsync(VerifyCodeDTO verifyCodeDTO)
        {
            bool isValid = await _verificationService.VerifyCodeAsync(verifyCodeDTO.Identifier, verifyCodeDTO.Code);

            if (!isValid)
                return await Task.FromResult(new VerifyCodeResponse { IsVerified = false, Message = "Invalid or expired verification code." });

            ApplicationUser? user = await _userService.FindUserByIdentifierAsync(verifyCodeDTO.Identifier, verifyCodeDTO.IdentifierType);

            if (verifyCodeDTO.IdentifierType == IdentifierEnum.email)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var result = await _userManager.ConfirmEmailAsync(user, token);

                if (!result.Succeeded)
                    return await Task.FromResult(new VerifyCodeResponse { IsVerified = false, Message = "Failed to confirm email." });

                await _profileService.SubscribeNotificationWayCommandAsync(user, NotificationWayEnum.Email);
            }
            else if (verifyCodeDTO.IdentifierType == IdentifierEnum.phone_number)
            {
                var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, verifyCodeDTO.Identifier);

                var result = await _userManager.ChangePhoneNumberAsync(user, verifyCodeDTO.Identifier, token);

                if (!result.Succeeded)
                    return await Task.FromResult(new VerifyCodeResponse { IsVerified = false, Message = "Failed to confirm phone number." });

                await _profileService.SubscribeNotificationWayCommandAsync(user, NotificationWayEnum.SMS);
            }

            await _profileService.SubscribeNotificationWayCommandAsync(user, NotificationWayEnum.Push_Notification);

            return await Task.FromResult(new VerifyCodeResponse { IsVerified = true, Message = "Verification successful." });
        }

        public async Task<bool> IsUserExistsCommandAsync(SendCodeDTO resendCodeDTO)
        {
            if (await _userService.FindUserByIdentifierAsync(resendCodeDTO.Identifier, resendCodeDTO.IdentifierType) != null)
                return await Task.FromResult(true);

            if (resendCodeDTO.IdentifierType == IdentifierEnum.email && await _userManager.FindByEmailAsync(resendCodeDTO.Identifier) != null)
                return await Task.FromResult(true);
            if (resendCodeDTO.IdentifierType == IdentifierEnum.phone_number && await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == resendCodeDTO.Identifier) != null)
                return await Task.FromResult(true);

            return await Task.FromResult(false);
        }

        public async Task<LogoutResponse> LogoutCommandAsync(LogoutCommand request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return new LogoutResponse { Success = false, Message = "User not found." };
            }

            var httpContext = _httpContextAccessor.HttpContext;
            var jti = httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Jti);
            if (!string.IsNullOrEmpty(jti))
            {
                var expClaim = httpContext.User.FindFirst("exp")?.Value;
                if (long.TryParse(expClaim, out var exp))
                {
                    var expiry = DateTimeOffset.FromUnixTimeSeconds(exp).UtcDateTime;
                    _blacklistService.AddToBlacklist(jti, expiry);
                }
            }

            // 2. Remove the refresh token cookie
            httpContext.Response.Cookies.Delete("RefreshToken");

            // 3. Revoke refresh tokens (all or the specific one)
            if (request.RevokeAll)
            {
                var tokens = await _refreshTokenRepo.GetActiveTokensForUserAsync(request.UserId);
                foreach (var token in tokens)
                {
                    token.IsRevoked = true;
                    token.RevokedByIp = "Logout all";
                    await _refreshTokenRepo.UpdateAsync(token);
                }
            }
            else
            {
                // Revoke only the one from the cookie
                var refreshToken = httpContext.Request.Cookies["RefreshToken"];
                if (!string.IsNullOrEmpty(refreshToken))
                    await _refreshTokenRepo.RevokeAsync(refreshToken, "Logout");
            }

            return new LogoutResponse { Success = true, Message = "Logged out successfully." };
        }
    }
}
