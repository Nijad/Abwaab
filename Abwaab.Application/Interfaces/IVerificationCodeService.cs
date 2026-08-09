using Abwaab.Application.Features.Users.Auth.SendCode;

namespace Abwaab.Application.Interfaces
{
    public interface IVerificationCodeService
    {
        string GenerateVerificationCode(); // Generates a 6-digit numeric code
        Task<SendCodeResponse> SendVerificationCodeAsync(SendCodeDTO resendCodeDTO);
        
        Task SendVerificationCodeViaEmailAsync(string email, string code);

        Task SendVerificationCodeViaSmsAsync(string phoneNo, string code);
        
        Task<bool> VerifyCodeAsync(string identifier, string userInputCode);
    }
}
