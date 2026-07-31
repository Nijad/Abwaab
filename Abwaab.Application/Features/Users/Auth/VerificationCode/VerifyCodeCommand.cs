namespace Abwaab.Application.Features.Users.Auth.VerificationCode
{
    public class VerifyCodeCommand
    {
        public string Identifier { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
