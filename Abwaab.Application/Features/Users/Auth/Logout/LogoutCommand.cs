using Abwaab.Application.Common.Interfaces;
using MediatR;

namespace Abwaab.Application.Features.Users.Auth.Logout
{
    public class LogoutCommand : IRequest<LogoutResponse> , IUserRequest
    {
        public Guid UserId { get; set; }
        // If not provided, we may revoke all
        public string? RefreshToken { get; set; }
        // If true, revoke all tokens for user
        public bool RevokeAll { get; set; }
    }
}
