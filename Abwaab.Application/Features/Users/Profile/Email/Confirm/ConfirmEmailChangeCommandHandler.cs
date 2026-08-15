using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Common.Exceptions.Profile.Email;
using Abwaab.Application.Common.Exceptions.Profile.VerificationCode;
using Abwaab.Application.Features.Users.Profile.Email.Pending;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Abwaab.Application.Features.Users.Profile.Email.Confirm
{
    public class ConfirmEmailChangeCommandHandler : IRequestHandler<ConfirmEmailChangeCommand, ConfirmEmailChangeResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserContext _userContext;
        private readonly IMemoryCache _cache;
        private readonly ILogger<ConfirmEmailChangeCommandHandler> _logger;
        private readonly string errorTitle = ErrorTitle.ConfirmEmailChange;

        public ConfirmEmailChangeCommandHandler(
            UserManager<ApplicationUser> userManager,
            IUserContext userContext,
            IMemoryCache cache,
            ILogger<ConfirmEmailChangeCommandHandler> logger)
        {
            _userManager = userManager;
            _userContext = userContext;
            _cache = cache;
            _logger = logger;
        }

        public async Task<ConfirmEmailChangeResponse> Handle(ConfirmEmailChangeCommand request, CancellationToken cancellationToken)
        {
            Guid userId = _userContext.UserId;
            ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                throw new UserNotFoundException(userId.ToString(), errorTitle);

            // Retrieve the pending change from cache
            var cacheKey = $"email_change_{userId}";
            if (!_cache.TryGetValue(cacheKey, out PendingEmailChange pending))
                throw new NoPendingEmailChangeException(errorTitle);

            // Validate the code and the new email
            if (pending.Code != request.Code || pending.NewEmail != request.NewEmail)
                throw new InvalidCodeOrEmailMissmatchException(errorTitle);

            // Check again if the email is still available (in case someone else took it while waiting)
            ApplicationUser? existingUser = await _userManager.FindByEmailAsync(request.NewEmail);
            
            if (existingUser != null && existingUser.Id != userId)
                throw new EmailAlreadyInUseException(errorTitle);

            // Store the old email before overwriting
            user.PreviousEmail = user.Email;
            // Update the user's email
            user.Email = request.NewEmail;
            // If you use email as username
            user.UserName = request.NewEmail;
            // Force re-verification of the new email
            user.EmailConfirmed = false;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                
                _logger.LogError("Failed to update user email for user {UserId}. Errors: {Errors}", userId, errors);

                throw new FailedConfirmationEmailException(errorTitle);
            }

            // Remove the cache entry (one-time use)
            _cache.Remove(cacheKey);

            return new ConfirmEmailChangeResponse { Success = true, Message = $"تغيير البريد الالكتروني تمت بنجاح" };
        }
    }
}
