namespace Abwaab.Application.Features.Users.Auth.RefreshToken
{
    public class RefreshTokenResponse
    {
        public bool Success { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
        public string? Message { get; set; }
    }
}
