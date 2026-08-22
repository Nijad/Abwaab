namespace Abwaab.Application.Features.Users.Profile.Phone.Pending
{
    public class PendingPhoneChange
    {
        public Guid UserId { get; set; }
        public string NewPhoneNo { get; set; } = string.Empty;
        public string OldPhoneNo { get; set; } = string.Empty;
        public string OldEmail { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Guid CancelCode { get; set; }
    }
}
