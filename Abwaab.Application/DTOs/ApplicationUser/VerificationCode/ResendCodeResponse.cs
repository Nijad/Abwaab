namespace Abwaab.Application.DTOs.ApplicationUser.VerificationCode
{
    public class ResendCodeResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
