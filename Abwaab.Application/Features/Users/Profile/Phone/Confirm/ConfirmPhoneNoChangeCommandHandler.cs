using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Common.Exceptions.Profile.Phone;
using Abwaab.Application.Common.Exceptions.Profile.VerificationCode;
using Abwaab.Application.Contracts;
using Abwaab.Application.Features.Users.Profile.Phone.Pending;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Abwaab.Application.Features.Users.Profile.Phone.Confirm
{
    public class ConfirmPhoneNoChangeCommandHandler : IRequestHandler<ConfirmPhoneNoChangeCommand, ConfirmPhoneNoChangeResponse>
    {
        private readonly IProfileService _profileService;
        private readonly IUserContext _userContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMemoryCache _cache;
        private readonly ILogger<ConfirmPhoneNoChangeCommandHandler> _logger;
        private readonly string errorTitle = ErrorTitle.ConfirmPhoneNoChange;

        public ConfirmPhoneNoChangeCommandHandler(IProfileService profileService, IUserContext userContext, UserManager<ApplicationUser> userManager, IMemoryCache cache, ILogger<ConfirmPhoneNoChangeCommandHandler> logger)
        {
            _profileService = profileService;
            _userContext = userContext;
            _userManager = userManager;
            _cache = cache;
            _logger = logger;
        }
        public async Task<ConfirmPhoneNoChangeResponse> Handle(ConfirmPhoneNoChangeCommand request, CancellationToken cancellationToken)
        {
            Guid userId = _userContext.UserId;
            ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                throw new UserNotFoundException(userId.ToString(), errorTitle);

            string cacheKey = $"phone_change_{userId}";
            if (!_cache.TryGetValue(cacheKey, out PendingPhoneChange pending))
                throw new NoPendingPhoneChangeException(errorTitle);

            if (pending.Code != request.Code || pending.NewPhoneNo != request.NewPhoneNo)
                throw new InvalidCodeOrPhoneMissmatchException(errorTitle);

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
            user.PhoneNumberConfirmed = false;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                
                _logger.LogError("Phone update failed for user {UserId}: {Errors}", userId, errors);
                
                throw new FailedConfirmationPhoneException(errorTitle);
            }

            // Remove the cache entry (one-time use)
            _cache.Remove(cacheKey);

            _logger.LogInformation("Phone number changed successfully for user {UserId} to {NewPhone}", userId, request.NewPhoneNo);
            return new ConfirmPhoneNoChangeResponse { Success = true, Message = "Phone number updated successfully." };



            //ConfirmPhoneNoChangeResponse response = await _profileService.ConfirmPhoneNoChangeCommandAsync(request);

            //return response;
        }
    }
}
