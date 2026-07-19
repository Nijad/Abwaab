namespace Abwaab.Application.DTOs.ApplicationUser
{
    public class VerifyCodeResponse
    {
        public bool IsVerified { get; set; }
        public string? Message { get; set; }
    }
}
