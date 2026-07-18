namespace Abwaab.Application.Common.Interfaces
{
    public interface IVerificationCodeService
    {
        string GenerateCode(); // Generates a 6-digit numeric code
        Task<bool> SendVerificationCodeAsync(string email, string phoneNumber, string code);
    }
}
