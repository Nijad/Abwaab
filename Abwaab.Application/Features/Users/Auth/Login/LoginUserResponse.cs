namespace Abwaab.Application.Features.Users.Auth.Login
{
    public class LoginUserResponse()
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
        public bool IsAdmin { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    };
}
