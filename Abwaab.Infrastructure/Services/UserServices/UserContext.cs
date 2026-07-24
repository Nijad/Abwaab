using Abwaab.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Abwaab.Infrastructure.Services.UserServices
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext?.User
                    .FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? _httpContextAccessor.HttpContext?.User
                    .FindFirstValue("sub");

                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("User ID not found in claims.");

                return new Guid(userId);
            }
        }

        public string? Email => _httpContextAccessor.HttpContext?.User
            .FindFirstValue(ClaimTypes.Email)
            ?? _httpContextAccessor.HttpContext?.User
            .FindFirstValue("email");

        public string? UserName => _httpContextAccessor.HttpContext?.User
            .FindFirstValue(ClaimTypes.Name)
            ?? _httpContextAccessor.HttpContext?.User
            .FindFirstValue("unique_name");

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public string? RemoteIpAddress => _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
    }
}
