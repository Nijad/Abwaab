namespace Abwaab.Application.Features.Users.Profile.Password.VerifyResetCode
{
    public class VerifyResetCodeResponse 
    {
        public DateTime ExpireAt { get; set; }
        public int CodeTimeOutInMinuts { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
