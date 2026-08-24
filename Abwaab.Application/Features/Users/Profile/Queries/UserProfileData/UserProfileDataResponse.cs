namespace Abwaab.Application.Features.Users.Profile.Queries.UserProfileData
{
    public class UserProfileDataResponse
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Identifier { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool EmailIsVerified { get; set; }
        public string MobileNumber { get; set; } = string.Empty;
        public bool MobileIsVerified { get; set; }
        public bool EmailNotificationStatus { get; set; }
        public bool SmsNotificationStatus { get; set; }
        public List<PendingChangeIdentifierDTO>? PendingChanges { get; set; }
    }
}
