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

        public async Task<LoginUserResponse> LoginUserAsync(LoginUserDTO loginUserDTO)
        {
            ApplicationUser? user;
            string? identifier;
            //IdentifierEnum loggingBy = IdentifierEnum.email;
            bool confirmed = false;

            user = _userManager.FindByNameAsync(loginUserDTO.Identifier).Result;

            if (user == null)
                throw new NotFoundException("User", loginUserDTO.IdentifierType.ToString().Replace('_', ' '), loginUserDTO.Identifier);

            if (loginUserDTO.IdentifierType == IdentifierEnum.email)
                confirmed = user.EmailConfirmed;
            else if (loginUserDTO.IdentifierType == IdentifierEnum.phone_number)
                confirmed = user.PhoneNumberConfirmed;

            bool checkPassword = await _userManager.CheckPasswordAsync(user, loginUserDTO.Password);

            if (!checkPassword)
                throw new InvalidPasswordException();

            if (!confirmed)
                return await Task.FromResult(new LoginUserResponse(false, $"Please verify your {loginUserDTO.IdentifierType.ToString().Replace('_', ' ')} before logging in."));

            return await Task.FromResult(new LoginUserResponse(true, "Login successful", await GenerateJwtTokenAsync(new ApplicationUserDTO
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email
            })));
        }

        public async Task<RegisterUserResponse> RegisterUserAsync(RegisterDTO registerDTO)
        {
            ApplicationUser? getUser = null;
            if (registerDTO.IdentifierType == IdentifierEnum.email)
            {
                getUser = await _userManager.FindByEmailAsync(registerDTO.Identifier);
            }

            if (registerDTO.IdentifierType == IdentifierEnum.phone_number)
            {
                getUser = await _userManager.Users
                                .FirstOrDefaultAsync(u => u.PhoneNumber == registerDTO.Identifier);
            }

            if (getUser != null)
                return await Task.FromResult(new RegisterUserResponse(false, "User already exists"));

            ApplicationUser newUser = new ApplicationUser
            {
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                UserName = registerDTO.Identifier,
            };

            if (registerDTO.IdentifierType == IdentifierEnum.email)
                newUser.Email = registerDTO.Identifier;
            else if (registerDTO.IdentifierType == IdentifierEnum.phone_number)
                newUser.PhoneNumber = registerDTO.Identifier;

            IdentityResult result = await _userManager.CreateAsync(newUser, registerDTO.Password);

            if (!result.Succeeded)
                return await Task.FromResult(new RegisterUserResponse(false, "Registration failed"));

            var code = _verificationService.GenerateCode();
            IdentifierDTO Identifier = new() { 
                Identifier = registerDTO.Identifier, 
                IdentifierType = registerDTO.IdentifierType 
            };

            var sent = await _verificationService.SendVerificationCodeAsync(Identifier, code);

            if (!sent)
                return await Task.FromResult(new RegisterUserResponse(false, "Failed to send verification code."));

            return await Task.FromResult(new RegisterUserResponse(true, $"Register Successful, Verification code sent to your {registerDTO.IdentifierType.ToString().Replace('_', ' ')}"));
        }

        public async Task<VerifyCodeResponse> VerifyUserAsync(VerifyCodeDTO verifyCodeDTO)
        {
            bool isValid = _verificationService.VerifyCodeAsync(verifyCodeDTO.Identifier, verifyCodeDTO.Code).Result;

            if (!isValid)
                return await Task.FromResult(new VerifyCodeResponse { IsVerified = false, Message = "Invalid or expired verification code." });

            ApplicationUser? user = _userManager.FindByNameAsync(verifyCodeDTO.Identifier).Result;
            
            if (verifyCodeDTO.IdentifierType == IdentifierEnum.email)
            {
                var token = _userManager.GenerateEmailConfirmationTokenAsync(user).Result;

                var result = _userManager.ConfirmEmailAsync(user, token).Result;

                if (!result.Succeeded)
                    return await Task.FromResult(new VerifyCodeResponse { IsVerified = false, Message = "Failed to confirm email." });
            }
            else if (verifyCodeDTO.IdentifierType == IdentifierEnum.phone_number)
            {
                var token = _userManager.GenerateChangePhoneNumberTokenAsync(user, verifyCodeDTO.Identifier).Result;

                var result = _userManager.ChangePhoneNumberAsync(user, verifyCodeDTO.Identifier, token).Result;

                if (!result.Succeeded)
                    return await Task.FromResult(new VerifyCodeResponse { IsVerified = false, Message = "Failed to confirm phone number." });
            }

            return await Task.FromResult(new VerifyCodeResponse { IsVerified = true, Message = "Verification successful." });
        }

        public async Task<bool> IsUserExistsAsync(IdentifierDTO resendCodeDTO)
        {
            if (_userManager.FindByNameAsync(resendCodeDTO.Identifier).Result != null)
                return await Task.FromResult(true);

            if (resendCodeDTO.IdentifierType == IdentifierEnum.email && _userManager.FindByEmailAsync(resendCodeDTO.Identifier).Result != null)
                return await Task.FromResult(true);
            if (resendCodeDTO.IdentifierType == IdentifierEnum.phone_number && _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == resendCodeDTO.Identifier).Result != null)
                return await Task.FromResult(true);

            return await Task.FromResult(false);
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordAsyn(ForgotPasswordDTO request)
        {
            var user = _userManager.FindByNameAsync(request.Identifier).Result;
            if (user == null)
                throw new NotFoundException("User", request.IdentifierType.ToString().Replace('_', ' '), request.Identifier);
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, resetToken, request.NewPassword);
            
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));

                return await Task.FromResult(new ForgotPasswordResponse {Success = false, Message = $"Password reset failed: {errors}"}); 
            }

             return new ForgotPasswordResponse { Success = true, Message = "Password reset successful." };
        }
    }
}
