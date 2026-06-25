using Abwaab.Application.DTOs.ApplicationUser;

namespace Abwaab.Application.Common.Contracts
{
    public interface IAuthService
    {
        Task<RegisterUserResponse> RegisterUserByEmailAsync(RegisterUserByEmailRequest registerRequest);
        Task<LoginUserResponse> LoginUserByEmailAsync(LoginUserByEmailRequest loginRequest);
    }
}
