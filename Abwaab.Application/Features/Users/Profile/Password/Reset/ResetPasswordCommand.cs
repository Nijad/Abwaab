namespace Abwaab.Application.Features.Users.Profile.Password.Reset
{
    public class ResetPasswordCommand
    {
        public string Identifier { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
