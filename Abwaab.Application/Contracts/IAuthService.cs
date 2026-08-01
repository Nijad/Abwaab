using Abwaab.Application.Features.Users.Auth.Logout;

namespace Abwaab.Application.Contracts
{
    public interface IAuthService
    {   
        Task<LogoutResponse> LogoutCommandAsync(LogoutCommand request);
    }
}
