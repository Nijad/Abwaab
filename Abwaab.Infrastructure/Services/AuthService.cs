using Abwaab.Application.Common.Contracts;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Interfaces;
using Abwaab.Application.DTOs.ApplicationUser;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Infrastructure.Options;
using Microsoft.AspNetCore.Identity;
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
                new System.Security.Claims.Claim("email", user.Email ?? ""),
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
            string? loginWith;
            bool confirmed = false;

            // Check if user requests to login with email or phone number
            if (!string.IsNullOrEmpty(loginRequest.Email))
            {
                user = _userManager.FindByEmailAsync(loginRequest.Email).Result;
                identifier = loginRequest.Email;
                loginWith = "email";
            }
            else if (!string.IsNullOrEmpty(loginRequest.PhoneNo))
            {
                user = _userManager.FindByNameAsync(loginRequest.PhoneNo).Result;
                identifier = loginRequest.PhoneNo;
                loginWith = "phone number";
            }
            else
                throw new ArgumentException("Either email or phone number must be provided.");
            //var user = await user;
            if (user == null)
                throw new NotFoundException("User", loginWith, identifier);

            if (!string.IsNullOrEmpty(loginRequest.Email))
                confirmed = user.EmailConfirmed;
            else if (!string.IsNullOrEmpty(loginRequest.PhoneNo))
                confirmed = user.PhoneNumberConfirmed;

            bool checkPassword = await _userManager.CheckPasswordAsync(user, loginRequest.Password);

            if (!checkPassword)
                throw new InvalidPasswordException();

            if (!confirmed)
                return await Task.FromResult(new LoginUserResponse(false, $"Please verify your {loginWith} before logging in."));

            return await Task.FromResult(new LoginUserResponse(true, "Login successful", await GenerateJwtTokenAsync(new ApplicationUserDTO
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email
            })));
        }

        public async Task<RegisterUserResponse> RegisterUserAsync(RegisterRequest registerRequest)
        {
            ApplicationUser? getUser = await _userManager.FindByEmailAsync(registerRequest.Email);

            if (getUser != null)
                return await Task.FromResult(new RegisterUserResponse(false, "User already exists"));

            ApplicationUser newUser = new ApplicationUser
            {
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                UserName = registerRequest.Username,
                Email = registerRequest.Email,
                PhoneNumber = registerRequest.PhoneNo
            };

            IdentityResult result = await _userManager.CreateAsync(newUser, registerRequest.Password);

            if (!result.Succeeded)
                return await Task.FromResult(new RegisterUserResponse(false, "Registration failed"));

            var code = _verificationService.GenerateCode();
            var sent = await _verificationService.SendVerificationCodeAsync(
                email: registerRequest.Email,
                phoneNumber: registerRequest.PhoneNo,
                code: code
            );

            if (!sent)
                return await Task.FromResult(new RegisterUserResponse(false, "Failed to send verification code."));

            return await Task.FromResult(new RegisterUserResponse(true, "Register Successful"));
        }

        public Task<VerifyCodeResponse> VerifyUserAsync(VerifyCodeRequest request)
        {
            bool isEmail = !string.IsNullOrEmpty(request.Email);
            string? identifier = isEmail ? request.Email : request.PhoneNumber;

            if (string.IsNullOrEmpty(identifier))
                throw new ArgumentException("Either email or phone number must be provided.");

            bool isValid = _verificationService.VerifyCodeAsync(identifier, request.Code, isEmail).Result;

            if (!isValid)
                return Task.FromResult(new VerifyCodeResponse { IsVerified = false, Message = "Invalid or expired verification code." });

            ApplicationUser? user;
            if (isEmail)
            {
                user = _userManager.FindByEmailAsync(identifier).Result;
                var token =  _userManager.GenerateEmailConfirmationTokenAsync(user).Result;
                var result =  _userManager.ConfirmEmailAsync(user, token).Result;
                if (!result.Succeeded)
                    return Task.FromResult(new VerifyCodeResponse { IsVerified = false, Message = "Failed to confirm email." });
            }
            else
            {
                user = _userManager.FindByNameAsync(identifier).Result;
                var token = _userManager.GenerateChangePhoneNumberTokenAsync(user, identifier).Result;
                var result = _userManager.ChangePhoneNumberAsync(user, identifier, token).Result;
                if (!result.Succeeded)
                    return Task.FromResult(new VerifyCodeResponse { IsVerified = false, Message = "Failed to confirm phone number." });
            }
            

            return Task.FromResult(new VerifyCodeResponse { IsVerified = true, Message = "Verification successful." });
        }
    }
}
