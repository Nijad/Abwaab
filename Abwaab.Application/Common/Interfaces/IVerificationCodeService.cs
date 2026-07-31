using Abwaab.Application.Features.Users.Auth.SendCode;

namespace Abwaab.Application.Common.Interfaces
{
    public interface IVerificationCodeService
    {
        string GenerateVerificationCode(); // Generates a 6-digit numeric code
        Task<SendCodeResponse> ResendVerificationCodeAsync(SendCodeDTO resendCodeDTO);
        
        Task<bool> SendVerificationCodeViaEmailAsync(string email, string code);

        Task<bool> SendVerificationCodeViaSmsAsync(string phoneNo, string code);
        
        Task<bool> VerifyCodeAsync(string identifier, string userInputCode);
    }
}
