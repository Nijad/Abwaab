namespace Abwaab.Application.DTOs.ApplicationUser.IdentifierManagement
{
    public class PendingEmailChange
    {
        public string NewEmail { get; set; }
        public string Code { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
