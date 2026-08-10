namespace Abwaab.Application.Features.Users.Profile.Password.Forgot
{
    public class ForgotPasswordResponse
    {
        public DateTime ExpireAt { get; set; }
        public int CodeTimeOutInMinuts { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}