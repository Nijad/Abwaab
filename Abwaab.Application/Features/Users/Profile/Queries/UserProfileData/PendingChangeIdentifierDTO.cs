namespace Abwaab.Application.Features.Users.Profile.Queries.UserProfileData
{
    public class PendingChangeIdentifierDTO
    {
        public string IdentifierType { get; set; } = string.Empty;
        public string Identifier { get; set; } = string.Empty;
        public Guid CancelCode { get; set; }
    }
}
