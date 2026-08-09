namespace Abwaab.Domain.Entities.UserEntities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string TokenHash { get; set; } = string.Empty;
        //public string Token { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? RevokedByIp { get; set; }
    }
}
