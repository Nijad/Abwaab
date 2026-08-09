namespace Abwaab.Application.Features.Users.Auth.Login
{
    public class TokenResponseDTO
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
    }
}
