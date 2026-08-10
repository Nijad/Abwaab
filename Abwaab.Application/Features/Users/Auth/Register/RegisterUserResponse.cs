namespace Abwaab.Application.Features.Users.Auth.Register
{
    public class RegisterUserResponse
    {
        public DateTime ExpireAt { get; set; }
        public int CodeTimeOutInMinuts { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; } = string.Empty;
    }
}
