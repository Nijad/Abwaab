using Abwaab.Application.DTOs.ApplicationUser;

namespace Abwaab.Application.Common.Interfaces
{
    public interface IVerificationCodeService
    {
        string GenerateCode(); // Generates a 6-digit numeric code
        Task<ResendCodeResponse> ResendVerificationCodeAsync(string identifier);
        Task<bool> SendVerificationCodeAsync(string email, string phoneNumber, string code);
        Task<bool> VerifyCodeAsync(string identifier, string userInputCode);
    }
}
