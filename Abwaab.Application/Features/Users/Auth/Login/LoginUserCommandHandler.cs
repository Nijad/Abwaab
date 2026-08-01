using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Common.Exceptions.Profile.Email;
using Abwaab.Application.Common.Exceptions.Profile.Phone;
using Abwaab.Application.Contracts;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Abwaab.Application.Features.Users.Auth.Login
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserDTO, LoginUserResponse>
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginUserCommandHandler(
            IUserService userService,
            IJwtService jwtService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userService = userService;
            _jwtService = jwtService;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<LoginUserResponse> Handle(LoginUserDTO request, CancellationToken cancellationToken)
        {
            ApplicationUser? user = await _userService.FindUserByIdentifierAsync(request.Identifier, request.IdentifierType);

            if (user == null)
                throw new NotFoundException(
                    "User", 
                    request.IdentifierType.ToString().Replace('_', ' '), 
                    request.Identifier);

            SignInResult result = await _signInManager.PasswordSignInAsync(user, request.Password, false, lockoutOnFailure: true);

            if (!result.Succeeded)
                throw new InvalidCredentialsException();

            if (result.IsLockedOut)
                throw new AccountLockedOutException();

            if (request.IdentifierType == IdentifierEnum.email && user.EmailConfirmed)
                throw new EmailNotVerifiedException();

            if (request.IdentifierType == IdentifierEnum.phone_number && user.PhoneNumberConfirmed)
                throw new PhoneNotVerifiedException();

            // null = unlock immediately
            await _userManager.SetLockoutEndDateAsync(user, null);
            // Reset failed attempts to 0
            await _userManager.ResetAccessFailedCountAsync(user);

            //get user roles
            IList<string> roles = await _userManager.GetRolesAsync(user);
            
            // Generate access token
            return await _jwtService.GenerateResponseAsync(user, roles);

        }
    }
}
