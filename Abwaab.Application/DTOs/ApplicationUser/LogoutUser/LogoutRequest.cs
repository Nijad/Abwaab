using MediatR;

namespace Abwaab.Application.DTOs.ApplicationUser.LogoutUser
{
    public class LogoutRequest : IRequest<LogoutResponse>
    {
        // If not provided, we may revoke all
        public string? RefreshToken { get; set; }
        // If true, revoke all tokens for user
        public bool RevokeAll { get; set; }
    }
}
