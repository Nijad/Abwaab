namespace Abwaab.Application.Features.Users.Profile.Password.Change
{
    public class ChangePasswordCommand
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
