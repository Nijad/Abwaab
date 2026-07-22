namespace Abwaab.Application.DTOs.ApplicationUser
{
    public class LoginUserResponse()
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
    };
}
