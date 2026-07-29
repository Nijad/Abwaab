namespace Abwaab.Application.Features.Users.Auth.Login
{
    public class LoginUserCommand
    {
        public string Identifier { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
