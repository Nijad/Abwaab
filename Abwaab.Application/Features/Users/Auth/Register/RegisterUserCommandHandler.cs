using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Contracts;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Domain.Enums;
using Abwaab.Infrastructure.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace Abwaab.Application.Features.Users.Auth.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserDTO, RegisterUserResponse>
    {
        private readonly IUserService _userService;
        private readonly IVerificationCodeService _verificationService;
        private readonly IMemoryCache _cache;
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterUserCommandHandler(
            IUserService userService,
            IVerificationCodeService verificationService,
            IMemoryCache cache,
            UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _verificationService = verificationService;
            _cache = cache;
            _userManager = userManager;
        }

        public async Task<RegisterUserResponse> Handle(RegisterUserDTO request, CancellationToken cancellationToken)
        {
            //check if user already exists
            ApplicationUser? user = await _userService.FindUserByIdentifierAsync(request.Identifier, request.IdentifierType);

            if (user != null)
                throw new UserAlreadyExistException();

            ApplicationUser newUser = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Identifier,
                LockoutEnabled = true
            };

            if (request.IdentifierType == IdentifierEnum.email)
                newUser.Email = request.Identifier;
            else if (request.IdentifierType == IdentifierEnum.phone_number)
                newUser.PhoneNumber = request.Identifier;

            IdentityResult result = await _userManager.CreateAsync(newUser, request.Password);

            if (!result.Succeeded)
                throw new RegistrationFailedException();

            string code = _verificationService.GenerateVerificationCode();

            if (request.IdentifierType == IdentifierEnum.email)
                await _verificationService.SendVerificationCodeViaEmailAsync(request.Identifier, code);
            else if (request.IdentifierType == IdentifierEnum.phone_number)
                await _verificationService.SendVerificationCodeViaSmsAsync(request.Identifier, code);

            // Store the code in cache with a 5-minute expiry
            _cache.Set(request.Identifier, code, TimeSpan.FromMinutes(Constants.CODE_TIMEOUT_MINUTES));

            return new RegisterUserResponse(true, $"Register Successful, Verification code sent to your {request.IdentifierType.ToString().Replace('_', ' ')}");
        }
    }
}
