namespace Abwaab.Application.DTOs.ApplicationUser
{
    public class ForgotPasswordRequest
    {
        public string Identifier { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
