using Abwaab.Application.Common.Contracts;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Interfaces;
using Abwaab.Application.DTOs.ApplicationUser;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Domain.Enums;
using Abwaab.Infrastructure.Common;
using Abwaab.Infrastructure.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Abwaab.Infrastructure.Identity.Services
{
    public class AuthService : IAuthService
    {
        private readonly IVerificationCodeService _verificationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOptions<JwtSettings> _jwtSettings;

        public AuthService(IVerificationCodeService verificationService, UserManager<ApplicationUser> userManager, IOptions<JwtSettings> jwtSettings)
        {
            _verificationService = verificationService;
            _userManager = userManager;
            _jwtSettings = jwtSettings;
        }

        private async Task<string> GenerateJwtTokenAsync(ApplicationUserDTO user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new System.Security.Claims.Claim("id", user.Id.ToString()),
                new System.Security.Claims.Claim("username", user.Username ?? "")
                // Add other claims as needed
            };
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Value.Issuer,
                audience: _jwtSettings.Value.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.Value.ExpiryMinutes),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<LoginUserResponse> LoginUserAsync(LoginUserRequest loginRequest)
        {
            ApplicationUser? user;
            string? identifier;
            IdentifierEnum loggingBy = IdentifierEnum.email;
            bool confirmed = false;

            // Check if user requests to login with email or phone number
            if (CommonValidation.IsValidPhoneNumber(loginRequest.Identifier))
                loggingBy = IdentifierEnum.phone_number;

            user = _userManager.FindByNameAsync(loginRequest.Identifier).Result;
                
            if (user == null)
                throw new NotFoundException("User", loggingBy.ToString().Replace('_',' '), loginRequest.Identifier);

            if (loggingBy == IdentifierEnum.email)
                confirmed = user.EmailConfirmed;
            else if (loggingBy == IdentifierEnum.phone_number)
                confirmed = user.PhoneNumberConfirmed;

            bool checkPassword = await _userManager.CheckPasswordAsync(user, loginRequest.Password);

            if (!checkPassword)
                throw new InvalidPasswordException();

            if (!confirmed)
                return await Task.FromResult(new LoginUserResponse(false, $"Please verify your {loggingBy.ToString().Replace('_', ' ')} before logging in."));

            return await Task.FromResult(new LoginUserResponse(true, "Login successful", await GenerateJwtTokenAsync(new ApplicationUserDTO
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email
            })));
        }

        public async Task<RegisterUserResponse> RegisterUserAsync(RegisterRequest registerRequest)
        {
            ApplicationUser? getUser = await _userManager.FindByEmailAsync(registerRequest.Identifier);

            if (getUser == null)
                getUser = await _userManager.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == registerRequest.Identifier);

            if (getUser != null)
                return await Task.FromResult(new RegisterUserResponse(false, "User already exists"));

            ApplicationUser newUser = new ApplicationUser
            {
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                UserName = registerRequest.Identifier,
            };

            IdentifierEnum userIdentifier = IdentifierEnum.email;
            if (CommonValidation.IsValidEmail(registerRequest.Identifier))
            {
                newUser.Email = registerRequest.Identifier;
                userIdentifier = IdentifierEnum.email;
            }
            else if (CommonValidation.IsValidPhoneNumber(registerRequest.Identifier))
            {
                newUser.PhoneNumber = registerRequest.Identifier;
                userIdentifier = IdentifierEnum.phone_number;
            }

            IdentityResult result = await _userManager.CreateAsync(newUser, registerRequest.Password);

            if (!result.Succeeded)
                return await Task.FromResult(new RegisterUserResponse(false, "Registration failed"));

            var code = _verificationService.GenerateCode();
            var sent = await _verificationService.SendVerificationCodeAsync(userIdentifier==IdentifierEnum.email?registerRequest.Identifier:string.Empty, userIdentifier == IdentifierEnum.phone_number ? registerRequest.Identifier : string.Empty, code);

            if (!sent)
                return await Task.FromResult(new RegisterUserResponse(false, "Failed to send verification code."));

            return await Task.FromResult(new RegisterUserResponse(true, $"Register Successful, Verification code sent to your {userIdentifier.ToString().Replace('_', ' ')}"));
        }

        public Task<VerifyCodeResponse> VerifyUserAsync(VerifyCodeRequest request)
        {
            IdentifierEnum identifierType = IdentifierEnum.email;
            if (CommonValidation.IsValidEmail(request.Identifier))
                identifierType = IdentifierEnum.email;
            else if (CommonValidation.IsValidPhoneNumber(request.Identifier))
                identifierType = IdentifierEnum.phone_number;

            bool isValid = _verificationService.VerifyCodeAsync(request.Identifier, request.Code).Result;

            if (!isValid)
                return Task.FromResult(new VerifyCodeResponse { IsVerified = false, Message = "Invalid or expired verification code." });

            ApplicationUser? user = _userManager.FindByNameAsync(request.Identifier).Result;
            if (identifierType == IdentifierEnum.email)
            {
                var token = _userManager.GenerateEmailConfirmationTokenAsync(user).Result;

                var result = _userManager.ConfirmEmailAsync(user, token).Result;

                if (!result.Succeeded)
                    return Task.FromResult(new VerifyCodeResponse { IsVerified = false, Message = "Failed to confirm email." });
            }
            else if (identifierType == IdentifierEnum.phone_number)
            {
                var token = _userManager.GenerateChangePhoneNumberTokenAsync(user, request.Identifier).Result;

                var result = _userManager.ChangePhoneNumberAsync(user, request.Identifier, token).Result;

                if (!result.Succeeded)
                    return Task.FromResult(new VerifyCodeResponse { IsVerified = false, Message = "Failed to confirm phone number." });
            }

            return Task.FromResult(new VerifyCodeResponse { IsVerified = true, Message = "Verification successful." });
        }
    }
}
