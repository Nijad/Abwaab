namespace Abwaab.Application.Features.Users.Auth.VerificationCode
{
    public class VerifyCodeResponse
    {
        public bool IsVerified { get; set; }
        public string? Message { get; set; }
    }
}
