using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Profile.Password;
using Abwaab.Application.Common.Exceptions.Profile.VerificationCode;
using Abwaab.Application.Contracts;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Domain.Enums;
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
                throw new InvalidVerificationCodeException();

            // 2. Find user
            var user = await _userService.FindUserByIdentifierAsync(request.Identifier, request.IdentifierType);
            if (user == null)
                throw new NotFoundException("User", request.IdentifierType.ToString().Replace("_", " "), request.Identifier);

            if (request.IdentifierType == IdentifierEnum.email)
            {
                if (user.PreviousEmail == request.Identifier)
                {
                    // Clear previous email if it matches the identifier
                    user.PreviousEmail = null;
                    // Update to the new email
                    user.Email  = request.Identifier;
                    // Mark email as unconfirmed
                    user.EmailConfirmed = false;
                }
            }
            else if (request.IdentifierType == IdentifierEnum.phone_number)
            {
                if (user.PreviousEmail == request.Identifier)
                {
                    // Clear previous email if it matches the identifier
                    user.PreviousPhoneNumber = null;
                    // Update to the new email
                    user.PhoneNumber = request.Identifier;
                    // Mark phone number as unconfirmed
                    user.PhoneNumberConfirmed = false;
                }
            }
            else
                throw new NotImplementedIdentifierException(request.IdentifierType.ToString().Replace("_", " "));

            //todo : enable if reset password does not update user information
            //await _userManager.UpdateAsync(user);


            // 3. Validate the code again
            var cacheKey = $"reset_{request.Identifier}";
            if (!_cache.TryGetValue(cacheKey, out string storedCode) || storedCode != request.Code)
                throw new InvalidVerificationCodeException();

            // 4. Generate password reset token via Identity
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, request.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Password reset failed for user {UserId}: {Errors}", user.Id, errors);

                throw new FailedResetPasswordException();
            }

            // 5. Clear cache entries
            _cache.Remove(cacheKey);
            _cache.Remove($"reset_verified_{request.Identifier}");



            return new ResetPasswordResponse { Success = true, Message = "Password has been reset." };
        }
    }
}
