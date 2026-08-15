using Abwaab.Application.Common.Constants;
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
        private readonly string errorTitle = ErrorTitle.LoggingUser;

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
            ApplicationUser? user = await _userService.FindUserByIdentifierAsync(request.Identifier, request.IdentifierType, errorTitle);

            if (user == null)
                throw new InvalidCredentialsException(errorTitle);

            SignInResult result = await _signInManager.PasswordSignInAsync(user, request.Password, false, lockoutOnFailure: true);
            
            if (result.IsLockedOut)
                throw new AccountLockedOutException(errorTitle);

            //todo: corrent error handling here
            if (!result.Succeeded)
                throw new InvalidCredentialsException(errorTitle);

            if ((request.IdentifierType == IdentifiersEnum.Email) && !user.EmailConfirmed)
                throw new EmailNotVerifiedException(errorTitle);

            if (request.IdentifierType == IdentifiersEnum.Phone_Number && !user.PhoneNumberConfirmed)
                throw new PhoneNotVerifiedException(errorTitle);

            // null = unlock immediately
            await _userManager.SetLockoutEndDateAsync(user, null);
            // Reset failed attempts to 0
            await _userManager.ResetAccessFailedCountAsync(user);

            //get user roles
            IList<string> roles = await _userManager.GetRolesAsync(user);

            // Generate access token
            TokenResponseDTO tokenResponse = await _jwtService.GenerateTokenResponseAsync(user, roles);

            // Check if user has Admin role
            bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            LoginUserResponse response = new LoginUserResponse
            {
                Success = true,
                Message = "تم تسجيل الدخول بنجاح",
                AccessToken = tokenResponse.AccessToken,
                RefreshToken = tokenResponse.RefreshToken,
                ExpiresIn = tokenResponse.ExpiresIn,
                IsAdmin = isAdmin,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return response;

        }
    }
}
