using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Enums;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Common.Exceptions.Profile.Email;
using Abwaab.Application.Contracts;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Whipstaff.Core.Entities;

namespace Abwaab.Application.Features.Users.Auth.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserDTO, RegisterUserResponse>
    {
        private readonly IUserService _userService;
        private readonly IVerificationCodeService _verificationService;
        private readonly IMemoryCache _cache;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterUserCommandHandler> _logger;
        private readonly string errorTitle = ErrorTitle.RegisterUser;

        public RegisterUserCommandHandler(
            IUserService userService,
            IVerificationCodeService verificationService,
            IMemoryCache cache,
            UserManager<ApplicationUser> userManager,
            ILogger<RegisterUserCommandHandler> logger)
        {
            _userService = userService;
            _verificationService = verificationService;
            _cache = cache;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<RegisterUserResponse> Handle(RegisterUserDTO request, CancellationToken cancellationToken)
        {
            //check if user already exists
            ApplicationUser? user = await _userService.FindUserByIdentifierAsync(request.Identifier, request.IdentifierType, errorTitle);

            if (user != null)
                throw new UserAlreadyExistException(errorTitle);

            ApplicationUser newUser = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Identifier,
                LockoutEnabled = true,
            };

            if (request.IdentifierType == IdentifiersEnum.Email)
                newUser.Email = request.Identifier;
            else if (request.IdentifierType == IdentifiersEnum.Phone_Number)
                newUser.PhoneNumber = request.Identifier;

            IdentityResult result = await _userManager.CreateAsync(newUser, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));

                _logger.LogError("Failed to register user. Errors: {Errors}", errors);

                throw new RegistrationFailedException(errorTitle);
            }

            string code = _verificationService.GenerateVerificationCode();

            if (request.IdentifierType == IdentifiersEnum.Email)
                await _verificationService.SendVerificationCodeViaEmailAsync(request.Identifier, code);
            else if (request.IdentifierType == IdentifiersEnum.Phone_Number)
                await _verificationService.SendVerificationCodeViaSmsAsync(request.Identifier, code);

            // Store the code in cache with a 5-minute expiry
            _cache.Set(request.Identifier, code, TimeSpan.FromMinutes(GeneralConstants.CODE_TIMEOUT_MINUTES));
            var response = new RegisterUserResponse
            {
                Success = true,
                Message = $"عملية التسجيل تمت بنجاح، وتم إرسال رمز التحقق إلى  '{request.Identifier}'",
                CodeTimeOutInMinuts = GeneralConstants.CODE_TIMEOUT_MINUTES,
                ExpireAt = DateTime.UtcNow.AddMinutes(GeneralConstants.CODE_TIMEOUT_MINUTES),
            };

            return response;
        }
    }
}
