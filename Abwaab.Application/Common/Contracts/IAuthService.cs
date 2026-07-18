using Abwaab.Application.DTOs.ApplicationUser;

namespace Abwaab.Application.Common.Contracts
{
    public interface IAuthService
    {
        Task<RegisterUserResponse> RegisterUserAsync(RegisterRequest registerRequest);

        //string GenerateVerificationCode(); // returns a 6‑digit code
        //Task SendVerificationCodeAsync(string email, string code);
        //Task SendVerificationCodeSmsAsync(string phoneNumber, string code);

        Task<LoginUserResponse> LoginUserByEmailAsync(LoginUserByEmailRequest loginRequest);
    }
}
