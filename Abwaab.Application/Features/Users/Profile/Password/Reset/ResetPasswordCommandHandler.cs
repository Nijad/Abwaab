using Abwaab.Application.Contracts;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Abwaab.Application.Features.Users.Profile.Password.Reset
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordDTO, ResetPasswordResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMemoryCache _cache;
        private readonly ILogger<ResetPasswordCommandHandler> _logger;
        private readonly IUserService _userService;

        public ResetPasswordCommandHandler(
            UserManager<ApplicationUser> userManager,
            IMemoryCache cache,
            ILogger<ResetPasswordCommandHandler> logger,
            IUserService userService)
        {
            _userManager = userManager;
            _cache = cache;
            _logger = logger;
            _userService = userService;
        }

        public async Task<ResetPasswordResponse> Handle(ResetPasswordDTO request, CancellationToken cancellationToken)
        {
            // 1. Ensure code was verified (optional but recommended)
            if (!_cache.TryGetValue($"reset_verified_{request.Identifier}", out bool _))
            {
                return new ResetPasswordResponse { Success = false, Message = "Code not verified. Please verify first." };
            }

            // 2. Find user
            var user = await _userService.FindUserByIdentifierAsync(request.Identifier, request.IdentifierType);
            if (user == null)
                return new ResetPasswordResponse { Success = false, Message = "User not found." };

            // 3. Validate the code again
            var cacheKey = $"reset_{request.Identifier}";
            if (!_cache.TryGetValue(cacheKey, out string storedCode) || storedCode != request.Code)
            {
                return new ResetPasswordResponse { Success = false, Message = "Invalid or expired code." };
            }

            // 4. Generate password reset token via Identity
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, request.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new ResetPasswordResponse { Success = false, Message = $"Reset failed: {errors}" };
            }

            // 5. Clear cache entries
            _cache.Remove(cacheKey);
            _cache.Remove($"reset_verified_{request.Identifier}");

            _logger.LogInformation("Password reset successfully for user {UserId}", user.Id);
            return new ResetPasswordResponse { Success = true, Message = "Password has been reset." };
        }
    }
}
