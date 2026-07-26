using Abwaab.Application.DTOs.ApplicationUser;
using Abwaab.Application.DTOs.ApplicationUser.VerificationCode;

namespace Abwaab.Application.Common.Interfaces
{
    public interface IVerificationCodeService
    {
        string GenerateVerificationCode(); // Generates a 6-digit numeric code
        Task<ResendCodeResponse> ResendVerificationCodeAsync(IdentifierDTO resendCodeDTO);
        
        Task<bool> SendVerificationCodeViaEmailAsync(string email, string code);

        Task<bool> SendVerificationCodeViaSmsAsync(string phoneNo, string code);
        
        Task<bool> VerifyCodeAsync(string identifier, string userInputCode);
    }
}
