namespace Abwaab.Application.Features.Users.Profile.Email.Pending
{
    public class PendingEmailChange
    {
        public string NewEmail { get; set; }
        public string Code { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
