using Abwaab.Application.Common.Contracts;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.DTOs.ApplicationUser;
using Abwaab.Domain.Entities.UserEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Abwaab.Infrastructure.Identity.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOptions<JwtSettings> _jwtSettings;

        public AuthService(UserManager<ApplicationUser> userManager, IOptions<JwtSettings> jwtSettings)
        {
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

        public async Task<LoginUserResponse> LoginUserByEmailAsync(LoginUserByEmailRequest loginRequest)
        {
            Task<ApplicationUser?> getUser = _userManager.FindByEmailAsync(loginRequest.Email);
            if (getUser.Result == null)
                throw new NotFoundException("User", "email", loginRequest.Email);

            bool checkPassword = await _userManager.CheckPasswordAsync(getUser.Result, loginRequest.Password);

            if (!checkPassword)
                throw new InvalidPasswordException();
                //return await Task.FromResult(new LoginUserResponse(false, "Invalid password"));
            
            return await Task.FromResult(new LoginUserResponse(true, "Login successful", await GenerateJwtTokenAsync(new ApplicationUserDTO
            {
                Id = getUser.Result.Id,
                Username = getUser.Result.UserName,
                Email = getUser.Result.Email
            })));
        }

        public async Task<RegisterUserResponse> RegisterUserByEmailAsync(RegisterUserByEmailRequest registerRequest)
        {
            ApplicationUser? getUser = await _userManager.FindByEmailAsync(registerRequest.Email);
            if (getUser != null)
            {
                return await Task.FromResult(new RegisterUserResponse(false, "User already exists"));
            }

            ApplicationUser newUser = new ApplicationUser
            {
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                UserName = registerRequest.Username,
                Email = registerRequest.Email
            };

            IdentityResult result = await _userManager.CreateAsync(newUser, registerRequest.Password);
            if (result.Succeeded)
            {
                return await Task.FromResult(new RegisterUserResponse(true, "Register Successful"));
            }

            return await Task.FromResult(new RegisterUserResponse(false, "Registration failed"));
        }
    }
}
